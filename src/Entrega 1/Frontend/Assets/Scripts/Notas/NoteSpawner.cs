using System.Collections;
using UnityEngine;

// VERSÃO PROVISÓRIA, só para a nova Note funcionar.
// Será substituída pelo "NoteSpawner de teste" (item 3), com spawnPoints, hitPoints e JudgmentSystem.
// Colocar em um GameObject VAZIO chamado "NoteSpawner" (não no prefab da nota).
public class NoteSpawner : MonoBehaviour
{
    [Header("Prefabs das notas: 1 por pista, na mesma ordem do Lane X")]
    public Note[] notePrefabs = new Note[4];

    [Header("Posição das pistas")]
    public float[] laneX = { -0.691f, 0.093f, 0.858f, 1.579f }; // X de cada pista
    public float spawnY = 0.5f;   // altura da nota (superfície da pista)
    public float spawnZ = 0f;     // onde a nota nasce (começo da pista)
    public float hitZ = 10f;      // onde fica a linha das teclas (fim da pista)

    [Header("Tempo")]
    public float travelTime = 2f;     // segundos da saída até a tecla
    public float spawnInterval = 1f;  // segundos entre notas

    [Header("Modo de teste")]
    public bool fixedLane = false;    // true = sempre a pista 0

    void Start()
    {
        SongClock.Begin(); // provisório: quem inicia o relógio da fase será definido depois
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        var wait = new WaitForSeconds(spawnInterval);

        while (true)
        {
            yield return wait;

            int lane = fixedLane ? 0 : Random.Range(0, laneX.Length);

            Note prefab = notePrefabs[lane];
            if (prefab == null)
            {
                Debug.LogWarning("NoteSpawner: falta um prefab no Note Prefabs, posição " + lane);
                continue;
            }

            Vector3 spawnPos = new Vector3(laneX[lane], spawnY, spawnZ);
            Vector3 hitPos = new Vector3(laneX[lane], spawnY, hitZ);

            // A nota deve estar na tecla travelTime segundos a partir de agora.
            double hitTime = SongClock.Now + travelTime;

            Note note = Instantiate(prefab, spawnPos, prefab.transform.rotation);
            note.Init(lane, hitTime, spawnPos, hitPos, travelTime);
        }
    }
}