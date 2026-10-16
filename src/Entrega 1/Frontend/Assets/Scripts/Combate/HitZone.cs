using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitZone : MonoBehaviour
{
    // Teclas que o jogador usa
    public Key[] teclas = { Key.D, Key.F, Key.K, Key.L };

    // Folga nas bordas do cubo. 0.25 = raio da bolinha, então acerta já ao encostar
    public float folga = 0.25f;

    // O cubo da HitZone (só pra definir a zona de acerto)
    public Collider cubo;

    // cópia da lista de notas que existem na tela agora
    List<Note> notas = new List<Note>();

    void Awake()
    {
        // Se o cubo não foi arrastado no Inspector, procura sozinho
        if (cubo == null)
        {
            GameObject objetoHitZone = GameObject.Find("HitZone");
            cubo = objetoHitZone.GetComponentInChildren<Collider>();
        }
    }

    void Update()
    {
        // Pega as notas que existem agora
        notas.Clear();
        foreach (Note nota in Note.Active)
        {
            if (nota != null && nota.Direction != Vector3.zero)
                notas.Add(nota);
        }

        // Para cada tecla, confere se acertou
        foreach (Key tecla in teclas)
        {
            if (Keyboard.current[tecla].wasPressedThisFrame)
            {
                Note notaNaZona = ProcurarNotaNaZona();

                if (notaNaZona != null)
                {
                    notaNaZona.Consume();   // a nota some
                    Debug.Log("ACERTOU!");
                }
                else
                {
                    Debug.Log("ERROU!");    // apertou antes ou depois da zona
                }
            }
        }

        // quando passa da zona
        foreach (Note nota in notas)
        {
            if (nota.Judged) continue;

            if (nota.transform.position.z > cubo.bounds.max.z + folga)
            {
                nota.Consume();
                Debug.Log("ERROU!");
            }
        }
    }

    // Devolve a nota que está dentro (ou encostando) no cubo. Se não tiver nenhuma, devolve null
    Note ProcurarNotaNaZona()
    {
        foreach (Note nota in notas)
        {
            if (nota.Judged) continue;

            float z = nota.transform.position.z;
            bool dentroDaZona = z >= cubo.bounds.min.z - folga && z <= cubo.bounds.max.z + folga;

            if (dentroDaZona) return nota;
        }
        return null;
    }
}