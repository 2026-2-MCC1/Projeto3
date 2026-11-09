using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitZone : MonoBehaviour
{
    // As teclas D F K L correspondem às 4 pistas (da esquerda para a direita na tela).
    public Key[] teclas = { Key.D, Key.F, Key.K, Key.L };

    // Pista (índice do NoteSpawner.laneX) de cada tecla: D = pista 3 ... L = pista 0.
    public int[] pistaDaTecla = { 3, 2, 1, 0 };
    public bool exigirPistaCorreta = true;

    public float espacoAMais = 0.25f;   // (mantido por compatibilidade com a cena)
    public Collider cubo;               // centro da zona de acerto

    [Header("Janelas de acerto (em segundos)")]
    public float janelaPerfeito = 0.09f;
    public float janelaBom = 0.22f;

    List<Note> notas = new List<Note>();

    float ZonaZ()
    {
        return cubo != null ? cubo.bounds.center.z : transform.position.z;
    }

    float Tempo(Note n, float zonaZ)
    {
        float v = Mathf.Max(0.01f, n.Speed);
        return (n.transform.position.z - zonaZ) / v;   // negativo = ainda não chegou
    }

    void Update()
    {
        if (GameSession.FaseEncerrada) return;

        notas.Clear();
        foreach (Note nota in Note.Active)
        {
            if (nota != null && !nota.Judged && nota.Direction != Vector3.zero) notas.Add(nota);
        }

        float zonaZ = ZonaZ();
        Keyboard kb = Keyboard.current;

        if (kb != null)
        {
            for (int i = 0; i < teclas.Length; i++)
            {
                if (!kb[teclas[i]].wasPressedThisFrame) continue;

                int pista = (i < pistaDaTecla.Length) ? pistaDaTecla[i] : i;
                GameEvents.LanePressed(pista);

                Note alvo = ProcurarNota(pista, zonaZ);
                if (alvo != null)
                {
                    float dt = Mathf.Abs(Tempo(alvo, zonaZ));
                    Judgment j = dt <= janelaPerfeito ? Judgment.Perfect : Judgment.Good;
                    alvo.Consume();
                    notas.Remove(alvo);
                    Julgar(pista, j);
                }
                else
                {
                    Julgar(pista, Judgment.Miss);
                }
            }
        }

        // notas que passaram da zona sem ser acertadas
        foreach (Note nota in notas)
        {
            if (nota.Judged) continue;
            if (Tempo(nota, zonaZ) > janelaBom)
            {
                nota.Consume();
                Julgar(nota.Lane, Judgment.Miss);
            }
        }
    }

    void Julgar(int pista, Judgment j)
    {
        GameEvents.LaneJudged(pista, j);
        GameEvents.NoteJudged(j);
    }

    Note ProcurarNota(int pista, float zonaZ)
    {
        Note melhor = null;
        float melhorDt = float.MaxValue;

        foreach (Note nota in notas)
        {
            if (nota.Judged) continue;
            if (exigirPistaCorreta && nota.Lane != pista) continue;

            float dt = Mathf.Abs(Tempo(nota, zonaZ));
            if (dt <= janelaBom && dt < melhorDt)
            {
                melhor = nota;
                melhorDt = dt;
            }
        }
        return melhor;
    }
}
