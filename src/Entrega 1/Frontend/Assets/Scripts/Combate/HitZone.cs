using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitZone : MonoBehaviour
{
    // Passo 1: escolher as teclas do jogo (D, F, K, L) e guardar as informações da zona de acerto:
    //          o cubo (arrastado no Inspector) onde a bolinha tem que estar e uma folga para acertar assim que ela encostar
    // Passo 2: anotar quais bolinhas estão na tela agora, para saber quais dá para acertar
    // Passo 3: ver se o jogador apertou alguma das teclas
    // Passo 4: se apertou e tinha bolinha na zona: foi acerto, a bolinha some e o combate é avisado
    // Passo 5: se apertou e não tinha bolinha na zona: foi erro e o combate é avisado
    // Passo 6: se a bolinha passou da zona sem o jogador apertar: foi erro, a bolinha some e o combate é avisado
    // Passo 7: criar um jeito de perguntar "tem alguma bolinha dentro da zona agora?".
    //          É usado quando o jogador aperta uma tecla, para decidir se foi acerto ou erro

    public Key[] teclas = { Key.D, Key.F, Key.K, Key.L };

    public float espacoAMais = 0.25f;

    public Collider cubo;

    List<Note> notas = new List<Note>();

    void Update()
    {
        notas.Clear();

        foreach (Note nota in Note.Active)
        {
            if (nota != null && nota.Direction != Vector3.zero)
            {
                notas.Add(nota);
            }
        }

        foreach (Key tecla in teclas)
        {
            if (Keyboard.current[tecla].wasPressedThisFrame)
            {
                Note notaNaZona = ProcurarNotaNaZona();

                if (notaNaZona != null)
                {
                    notaNaZona.Consume();
                    GameEvents.NoteJudged(Judgment.Perfect);
                }
                else
                {
                    GameEvents.NoteJudged(Judgment.Miss);
                }
            }
        }

        foreach (Note nota in notas)
        {
            if (nota.Judged == false)
            {
                if (nota.transform.position.z > cubo.bounds.max.z + espacoAMais)
                {
                    nota.Consume();
                    GameEvents.NoteJudged(Judgment.Miss);
                }
            }
        }
    }

    Note ProcurarNotaNaZona()
    {
        foreach (Note nota in notas)
        {
            if (nota.Judged == false)
            {
                float z = nota.transform.position.z;
                bool dentroDaZona = z >= cubo.bounds.min.z - espacoAMais && z <= cubo.bounds.max.z + espacoAMais;

                if (dentroDaZona == true)
                {
                    return nota;
                }
            }
        }

        return null;
    }
}