using UnityEngine;

// Colocar no PREFAB da nota. A posição vem do SongClock, não de soma por quadro.
// Movimento retilíneo uniforme: t = 1 no spawn, t = 0 na tecla, t < 0 depois dela.
[RequireComponent(typeof(Rigidbody))]
public class Note : MonoBehaviour
{
    [Tooltip("Rede de segurança: destrói a nota X segundos depois do hitTime caso ninguém a tenha destruído (o JudgmentSystem faz isso antes).")]
    [SerializeField] private float safetyDestroyAfter = 3f;

    private int lane;
    private double hitTime;
    private Vector3 spawnPos;
    private Vector3 hitPos;
    private float travelTime;
    private bool initialized;

    // Pista (0 a 3) e segundo do SongClock em que a nota deve estar na tecla.
    public int Lane => lane;
    public double HitTime => hitTime;

    private void Awake()
    {
        // Física desligada: a nota só obedece ao relógio.
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        // Collider como trigger (não empurra nada).
        var col = GetComponent<Collider>();
        if (col != null) col.isTrigger = true;
    }

    // Chamado pelo spawner logo depois de criar a nota.
    public void Init(int lane, double hitTime, Vector3 spawnPos, Vector3 hitPos, float travelTime)
    {
        this.lane = lane;
        this.hitTime = hitTime;
        this.spawnPos = spawnPos;
        this.hitPos = hitPos;
        this.travelTime = Mathf.Max(0.01f, travelTime); // evita dividir por zero
        initialized = true;

        UpdatePosition(); // já nasce na posição certa, sem esperar o próximo quadro
    }

    private void Update()
    {
        if (!initialized) return;

        UpdatePosition();

        if (SongClock.Now > hitTime + safetyDestroyAfter)
            Destroy(gameObject);
    }

    private void UpdatePosition()
    {
        // t = 1 no spawn, 0 na tecla, negativo depois da tecla.
        float t = (float)((hitTime - SongClock.Now) / travelTime);
        transform.position = Vector3.LerpUnclamped(hitPos, spawnPos, t);
    }
}