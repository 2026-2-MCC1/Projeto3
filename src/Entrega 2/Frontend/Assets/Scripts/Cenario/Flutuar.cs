using UnityEngine;

// Faz um objeto de cenário flutuar, girar e/ou derivar suavemente.
public class Flutuar : MonoBehaviour
{
    public float amplitude = 0.3f;
    public float velocidade = 1f;
    public float giro = 0f;                 // graus por segundo no eixo Y
    public Vector3 deriva = Vector3.zero;   // vai e volta nessa direção

    Vector3 p0;
    float fase;

    void Start()
    {
        p0 = transform.position;
        fase = Random.value * 6.2831f;
    }

    void Update()
    {
        float t = Time.time;
        transform.position = p0
            + Vector3.up * Mathf.Sin(t * velocidade + fase) * amplitude
            + deriva * Mathf.Sin(t * velocidade * 0.35f + fase);
        if (Mathf.Abs(giro) > 0.01f) transform.Rotate(0f, giro * Time.deltaTime, 0f, Space.World);
    }
}
