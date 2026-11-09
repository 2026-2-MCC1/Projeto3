using System.Collections;
using UnityEngine;

// Cria as notas. Dois modos:
//  - aleatório (padrão antigo): sorteia pista e intervalo;
//  - música (usarMusica): cada nota CHEGA na zona de acerto no segundo exato da trilha (lista "tempos").
public class NoteSpawner : MonoBehaviour
{
    [Header("Prefabs das notas: 1 por pista, na mesma ordem do Lane X")]
    public Note[] notePrefabs = new Note[4];

    [Header("Posição de saída")]
    public float[] laneX = { -0.691f, 0.093f, 0.858f, 1.579f };
    public float spawnY = 0.5f;
    public float spawnZ = 3.2f;

    [Header("Movimento da nota")]
    public float noteSpeed = 3f;
    public Vector3 moveDirection = Vector3.forward;
    public float noteLifetime = 6f;

    [Header("Tempo entre notas (modo aleatório)")]
    public float minInterval = 0.5f;
    public float maxInterval = 1.5f;
    public float startDelay = 0f;

    [Header("Modo de spawn")]
    public bool randomLanes = true;
    public bool loopPattern = true;
    public int[] pattern = { 1, 2, 3, 4, 2, 3, 2, 1, 2, 2, 3, 4, 3, 1, 2, 4, 3, 1, 2, 4, 1, 1, 3, 1, 4 };

    [Header("Modo música (notas sincronizadas com a trilha)")]
    public bool usarMusica = false;
    public AudioSource musica;
    public float[] tempos;      // segundos da trilha em que a nota deve chegar na zona
    public int[] pistas;        // pista (0..3) de cada nota

    int patternIndex = 0;
    int indiceMusica = 0;
    float ultimoT = 0f;
    float viagem = 2f;
    bool running = true;

    void OnEnable()
    {
        GameEvents.OnBossDefeated += Stop;
        GameEvents.OnPlayerDefeated += Stop;
    }

    void OnDisable()
    {
        GameEvents.OnBossDefeated -= Stop;
        GameEvents.OnPlayerDefeated -= Stop;
    }

    void Stop(int _) { running = false; }
    void Stop() { running = false; }

    void Start()
    {
        if (notePrefabs.Length != laneX.Length)
            Debug.LogWarning("NoteSpawner: o número de Note Prefabs precisa ser igual ao número de Lane X.");

        if (usarMusica && tempos != null && tempos.Length > 0)
        {
            if (musica == null) musica = FindFirstObjectByType<AudioSource>();

            float zonaZ = 10.44f;
            HitZone hz = FindFirstObjectByType<HitZone>();
            if (hz != null && hz.cubo != null) zonaZ = hz.cubo.bounds.center.z;

            float vz = Mathf.Abs(moveDirection.normalized.z) * noteSpeed;
            viagem = Mathf.Abs(zonaZ - spawnZ) / Mathf.Max(0.01f, vz);
        }
        else
        {
            usarMusica = false;
            StartCoroutine(SpawnLoop());
        }
    }

    void Update()
    {
        if (!usarMusica || !running) return;
        if (musica == null || !musica.isPlaying) return;

        float t = musica.time;
        if (t < ultimoT - 0.5f) indiceMusica = 0;   // a trilha recomeçou (loop)
        ultimoT = t;

        while (indiceMusica < tempos.Length)
        {
            float quando = tempos[indiceMusica] - viagem;
            if (quando > t) break;

            if (t - quando < 0.35f)
            {
                int lane = (pistas != null && indiceMusica < pistas.Length) ? pistas[indiceMusica] : Random.Range(0, laneX.Length);
                Spawn(Mathf.Clamp(lane, 0, laneX.Length - 1));
            }
            indiceMusica++;
        }
    }

    IEnumerator SpawnLoop()
    {
        if (startDelay > 0f) yield return new WaitForSeconds(startDelay);

        while (running)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            if (!running) yield break;

            int lane = GetNextLane();
            if (lane < 0) yield break;
            Spawn(lane);
        }
    }

    void Spawn(int lane)
    {
        Note prefab = notePrefabs[lane];
        if (prefab == null)
        {
            Debug.LogWarning("NoteSpawner: falta um prefab no Note Prefabs, posição " + lane);
            return;
        }

        Vector3 pos = new Vector3(laneX[lane], spawnY, spawnZ);
        Note note = Instantiate(prefab, pos, prefab.transform.rotation);
        note.Init(noteSpeed, moveDirection, Mathf.Max(noteLifetime, viagem + 2f), lane);
    }

    int GetNextLane()
    {
        if (randomLanes)
            return Random.Range(0, laneX.Length);

        if (patternIndex >= pattern.Length)
        {
            if (!loopPattern) return -1;
            patternIndex = 0;
        }

        int lane = pattern[patternIndex] - 1;
        patternIndex++;
        return Mathf.Clamp(lane, 0, laneX.Length - 1);
    }
}
