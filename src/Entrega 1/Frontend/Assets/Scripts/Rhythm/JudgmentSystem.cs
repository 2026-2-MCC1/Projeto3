using UnityEngine;

// Compara a posição da nota com a zona de acerto e dispara Perfeito/Bom/Errou.
public class JudgmentSystem : MonoBehaviour
{
    public Transform hitZone;
    public float perfectWindow = 0.25f;   // unidades do Unity
    public float goodWindow = 0.6f;
    public bool emptyTapIsMiss = false;   // apertar sem nota por perto conta como erro?

    void OnEnable() { GameEvents.OnLanePressed += HandleLane; }
    void OnDisable() { GameEvents.OnLanePressed -= HandleLane; }

    // >0 = nota ainda não chegou; <0 = já passou da zona
    float SignedDistance(Note n)
    {
        return Vector3.Dot(hitZone.position - n.transform.position, n.Direction);
    }

    void HandleLane(int lane)
    {
        if (GameStateManager.Current != GameState.Playing) return;

        Note best = null;
        float bestDist = float.MaxValue;

        foreach (Note n in Note.Active)
        {
            if (n.Judged || n.Lane != lane) continue;
            float a = Mathf.Abs(SignedDistance(n));
            if (a < bestDist) { bestDist = a; best = n; }
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
        if (GameStateManager.Current != GameState.Playing) return;

        // nota que passou da zona sem ser tocada = Errou
        foreach (Note n in Note.Active)
        {
            if (n.Judged) continue;
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
