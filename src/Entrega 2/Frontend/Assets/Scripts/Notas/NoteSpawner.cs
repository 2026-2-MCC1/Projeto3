using System.Collections;
using UnityEngine;

// Mesmo spawner da Pessoa 1, agora passa a pista para a nota e para quando a fase acaba.
public class NoteSpawner : MonoBehaviour
{
    [Header("Prefabs das notas: 1 por pista, na mesma ordem do Lane X")]
    public Note[] notePrefabs = new Note[4];

    [Header("Posição de saída")]
    public float[] laneX = { -0.691f, 0.093f, 0.858f, 1.579f };
    public float spawnY = 0.5f;  // altura da nota (use a altura da superfície da pista)
    public float spawnZ = 3.2f;  // ponto de saída no começo da pista

    [Header("Movimento da nota")]
    public float noteSpeed = 3f;  // velocidade (unidades por segundo)
    public Vector3 moveDirection = Vector3.forward; // para onde a nota anda
    public float noteLifetime = 6f;  // segundos até a nota ser destruída

    [Header("Tempo entre notas")]
    public float minInterval = 0.5f;
    public float maxInterval = 1.5f;

    [Header("Modo de spawn")]
    public bool randomLanes = true;  // true = pista aleatória, false = segue o pattern
    public bool loopPattern = true;  // repete o pattern quando acabar
    public int[] pattern = { 1, 2, 3, 4, 2, 3, 2, 1, 2, 2, 3, 4, 3, 1, 2, 4, 3, 1, 2, 4, 1, 1, 3, 1, 4 };

    int patternIndex = 0;
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

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (running)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            if (!running) yield break;

            int lane = GetNextLane();
            if (lane < 0) yield break;  // pattern acabou e loopPattern está desligado

            Note prefab = notePrefabs[lane];  // o prefab da pista sorteada = a cor da pista
            if (prefab == null)
            {
                Debug.LogWarning("NoteSpawner: falta um prefab no Note Prefabs, posição " + lane);
                continue;
            }

            Vector3 pos = new Vector3(laneX[lane], spawnY, spawnZ);
            Note note = Instantiate(prefab, pos, prefab.transform.rotation);
            note.Init(noteSpeed, moveDirection, noteLifetime, lane);
        }
    }

    // Devolve a pista (0 a 3) da próxima nota.
    int GetNextLane()
    {
        if (randomLanes)
            return Random.Range(0, laneX.Length);

        if (patternIndex >= pattern.Length)
        {
            if (!loopPattern) return -1;
            patternIndex = 0;
        }

        int lane = pattern[patternIndex] - 1;  // o pattern usa 1 a 4, o array começa em 0
        patternIndex++;
        return Mathf.Clamp(lane, 0, laneX.Length - 1);
    }
}