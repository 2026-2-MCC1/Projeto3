using System.Collections.Generic;
using UnityEngine;

// Compara a posição da nota com a zona de acerto e dispara Perfeito/Bom/Errou.
// A zona de acerto agora é o CUBO VISÍVEL da HitZone (o Collider filho), e não só o ponto central.
// Regra: se a bolinha está dentro do cubo quando a tecla da pista dela é apertada = ACERTOU.
public class JudgmentSystem : MonoBehaviour
{
    public Transform hitZone;

    public Collider hitZoneCollider;

    public float perfectWindow = 0.25f;

    public float goodWindow = 0.6f;

    public float edgeTolerance = 0.25f;

    public bool anyKeyHitsAnyLane = true;

    // Mantido só por compatibilidade com as cenas. Apertar sem nota na zona SEMPRE é erro.
    public bool emptyTapIsMiss = true;

    readonly List<Note> buffer = new List<Note>();

    void OnEnable() { GameEvents.OnLanePressed += HandleLane; }
    void OnDisable() { GameEvents.OnLanePressed -= HandleLane; }

    void Awake()
    {
        if (hitZoneCollider == null && hitZone != null)
            hitZoneCollider = hitZone.GetComponentInChildren<Collider>();
    }

    // Centro da zona (centro real do cubo, não o pivô do objeto)
    Vector3 ZoneCenter()
    {
        return hitZoneCollider != null ? hitZoneCollider.bounds.center : hitZone.position;
    }

    // Metade da profundidade da zona ao longo da direção em que a nota anda
    float ZoneHalfDepth(Vector3 dir)
    {
        if (hitZoneCollider == null) return goodWindow;
        Vector3 e = hitZoneCollider.bounds.extents;
        return Mathf.Abs(dir.x) * e.x + Mathf.Abs(dir.y) * e.y + Mathf.Abs(dir.z) * e.z;
    }

    // > 0 = a nota ainda não chegou ao centro; < 0 = já passou do centro
    float SignedDistance(Note n)
    {
        return Vector3.Dot(ZoneCenter() - n.transform.position, n.Direction);
    }

    // A nota está dentro da zona (cubo + tolerância)?
    bool IsInside(Note n)
    {
        return Mathf.Abs(SignedDistance(n)) <= ZoneHalfDepth(n.Direction) + edgeTolerance;
    }

    void HandleLane(int lane)
    {
        if (hitZone == null) return;

        Note best = null;
        float bestDist = float.MaxValue;

        buffer.Clear();
        buffer.AddRange(Note.Active);

        foreach (Note n in buffer)
        {
            if (n == null || n.Judged) continue;
            if (!anyKeyHitsAnyLane && n.Lane != lane) continue;
            if (n.Direction == Vector3.zero) continue;   // nota parada na cena: ignora
            if (!IsInside(n)) continue;                  // fora da zona: não conta

            float a = Mathf.Abs(SignedDistance(n));
            if (a < bestDist)
            {
                bestDist = a;
                best = n;
            }
        }

        // nenhuma bolinha dentro/encostada na zona = clicou antes ou depois = ERROU
        if (best == null)
        {
            GameEvents.NoteJudged(Judgment.Miss);
            return;
        }

        best.Consume();
        GameEvents.NoteJudged(bestDist <= perfectWindow ? Judgment.Perfect : Judgment.Good);
    }

    void Update()
    {
        if (hitZone == null) return;

        buffer.Clear();
        buffer.AddRange(Note.Active);

        foreach (Note n in buffer)
        {
            if (n == null || n.Judged) continue;
            if (n.Direction == Vector3.zero) continue;

            // nota que saiu completamente da zona sem ser tocada = Errou
            if (SignedDistance(n) < -(ZoneHalfDepth(n.Direction) + edgeTolerance))
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
        Gizmos.DrawWireSphere(ZoneCenter(), perfectWindow);

        Collider c = hitZoneCollider != null ? hitZoneCollider : hitZone.GetComponentInChildren<Collider>();
        if (c != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(c.bounds.center, c.bounds.size);
        }
    }
}