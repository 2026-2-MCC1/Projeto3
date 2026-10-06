using System.Collections;
using UnityEngine;

// Colocar em um GameObject VAZIO chamado "NoteSpawner" (não no prefab da nota).
public class NoteSpawner : MonoBehaviour
{
    [Header("Prefab da nota (precisa ter o script Note)")]
    public Note notePrefab;

    [Header("Posição de saída")]
    public float[] laneX = { -0.691f, 0.093f, 0.858f, 1.579f }; // X de cada pista
    public float spawnY = 0f;   // altura da nota (use a altura da superfície da pista)
    public float spawnZ = 0f;   // ponto de saída no começo da pista

    [Header("Movimento da nota")]
    public float noteSpeed = 0.5f;                    // velocidade (unidades por segundo)
    public Vector3 moveDirection = Vector3.forward;    // para onde a nota anda (0,0,-1)
    public float noteLifetime = 6f;                 // segundos até a nota ser destruída

    [Header("Tempo entre notas")]
    public float minInterval = 0.5f;
    public float maxInterval = 1.5f;

    [Header("Modo de spawn")]
    public bool randomLanes = true;                 // true = pista aleatória, false = segue o pattern
    public bool loopPattern = true;                 // repete o pattern quando acabar
    public int[] pattern = { 1, 2, 3, 4, 2, 3, 2, 1, 2, 2, 3, 4, 3, 1, 2, 4, 3, 1, 2, 4, 1, 1, 3, 1, 4 };

    int patternIndex = 0;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            int lane = GetNextLane();
            if (lane < 0) yield break; // pattern acabou e loopPattern está desligado

            Vector3 pos = new Vector3(laneX[lane], spawnY, spawnZ);
            Note note = Instantiate(notePrefab, pos, notePrefab.transform.rotation);
            note.Init(noteSpeed, moveDirection, noteLifetime);
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

        int lane = pattern[patternIndex] - 1; // o pattern usa 1 a 4, o array começa em 0
        patternIndex++;
        return Mathf.Clamp(lane, 0, laneX.Length - 1);
    }
}