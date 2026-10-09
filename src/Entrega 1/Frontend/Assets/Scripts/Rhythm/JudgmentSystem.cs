using System.Collections.Generic;
using UnityEngine;

// Compara a posição da nota com a zona de acerto e dispara Perfeito/Bom/Errou.
public class JudgmentSystem : MonoBehaviour
{
    public Transform hitZone;
    public float perfectWindow = 0.25f;   
    public float goodWindow = 0.6f;
    public bool emptyTapIsMiss = false;   // apertar sem nota por perto conta como erro?

    // cópia da lista de notas, para poder consumir notas sem quebrar o foreach
    readonly List<Note> buffer = new List<Note>();

    void OnEnable() { GameEvents.OnLanePressed += HandleLane; }
    void OnDisable() { GameEvents.OnLanePressed -= HandleLane; }

    // > 0 = a nota ainda não chegou; < 0 = já passou da zona
    float SignedDistance(Note n)
    {
        return Vector3.Dot(hitZone.position - n.transform.position, n.Direction);
    }

    void HandleLane(int lane)
    {
        if (hitZone == null) return;
        if (GameStateManager.Current != GameState.Playing) return;

        Note best = null;
        float bestDist = float.MaxValue;

        buffer.Clear();
        buffer.AddRange(Note.Active);

        foreach (Note n in buffer)
        {
            if (n == null || n.Judged || n.Lane != lane) continue;

            float a = Mathf.Abs(SignedDistance(n));
            if (a < bestDist)
            {
                bestDist = a;
                best = n;
            }
        }

        if (best == null || bestDist > goodWindow)
        {
            if (emptyTapIsMiss) GameEvents.NoteJudged(Judgment.Miss);
            return;
        }

        best.Consume();
        GameEvents.NoteJudged(bestDist <= perfectWindow ? Judgment.Perfect : Judgment.Good);
    }

    void Update()
    {
        if (hitZone == null) return;
        if (GameStateManager.Current != GameState.Playing) return;

        // copia a lista: Consume() pode alterar Note.Active durante o loop
        buffer.Clear();
        buffer.AddRange(Note.Active);

        foreach (Note n in buffer)
        {
            if (n == null || n.Judged) continue;

            // nota que passou da zona sem ser tocada = Errou
            if (SignedDistance(n) < -goodWindow)
            {
                n.Consume();
                GameEvents.NoteJudged(Judgment.Miss);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (hitZone == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(hitZone.position, perfectWindow);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hitZone.position, goodWindow);
    }
}