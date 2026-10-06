using UnityEngine;

// Colocar no PREFAB da nota (a bola/doce que anda pela pista).
// Ela anda em linha reta (sem gravidade, sem precisar inclinar a pista)
// e se destrói sozinha depois de alguns segundos.
public class Note : MonoBehaviour
{
    float speed;
    Vector3 direction;

    // Chamado pelo NoteSpawner logo depois de criar a nota.
    public void Init(float speed, Vector3 direction, float lifetime)
    {
        this.speed = speed;
        this.direction = direction.normalized;

        // Remove o objeto da cena e libera a memória depois de 'lifetime' segundos.
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Movimento constante (MRU): deslocamento = velocidade x tempo do quadro.
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}