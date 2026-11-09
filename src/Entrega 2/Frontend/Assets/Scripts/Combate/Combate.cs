using UnityEngine;

public class Combate : MonoBehaviour
{
    public int vidaBoss = 250;
    public int danoPorAcerto = 10;     // dano de um acerto Perfeito (Bom causa 60%)
    public int vidasJogador = 3;
    public int pontosJogador = 10;     // pontos base de um Perfeito (Bom vale metade)

    [Header("Bônus de sequência de Perfeitos")]
    public int comboPerfeitos = 3;     // quantos Perfeitos seguidos
    public int danoCombo = 10;         // dano extra ao completar a sequência

    int vidaBossMax;
    int vidasMax;
    bool terminou = false;
    int sequenciaPerfeitos = 0;
    int fase = 1;

    void Awake()
    {
        fase = GameSession.FaseDaCenaAtual();
        GameSession.IniciarFase(fase);
    }

    void Start()
    {
        vidaBossMax = Mathf.Max(1, vidaBoss);
        vidasMax = Mathf.Max(1, vidasJogador);
        GameEvents.PlayerLifeLost(vidasJogador, vidasMax);
        GameEvents.BossHealthChanged(vidaBoss, vidaBossMax);
        GameEvents.ScoreChanged(0, 0, 1);
    }

    void OnEnable()
    {
        GameEvents.OnNoteJudged += AoJulgarNota;
    }

    void OnDisable()
    {
        GameEvents.OnNoteJudged -= AoJulgarNota;
    }

    void AoJulgarNota(Judgment julgamento)
    {
        if (terminou) return;

        GameSession.Registrar(julgamento, pontosJogador);
        GameEvents.ScoreChanged(GameSession.Pontos, GameSession.Combo, GameSession.Multiplicador);

        if (julgamento == Judgment.Miss)
        {
            sequenciaPerfeitos = 0;
            vidasJogador--;
            GameEvents.PlayerLifeLost(Mathf.Max(0, vidasJogador), vidasMax);

            if (vidasJogador <= 0)
            {
                terminou = true;
                GameSession.EncerrarFase();
                GameEvents.PlayerDefeated();
            }
        }
        else
        {
            int dano = (julgamento == Judgment.Perfect) ? danoPorAcerto : Mathf.Max(1, danoPorAcerto * 6 / 10);

            if (julgamento == Judgment.Perfect)
            {
                sequenciaPerfeitos++;
                if (comboPerfeitos > 0 && sequenciaPerfeitos >= comboPerfeitos)
                {
                    sequenciaPerfeitos = 0;
                    dano += danoCombo;
                }
            }
            else
            {
                sequenciaPerfeitos = 0;
            }

            vidaBoss = Mathf.Max(0, vidaBoss - dano);
            GameEvents.BossHealthChanged(vidaBoss, vidaBossMax);

            if (vidaBoss <= 0)
            {
                terminou = true;
                GameSession.ConcluirFase();
                GameSession.EncerrarFase();
                GameEvents.BossDefeated(fase);
                GameEvents.PhaseCompleted(fase);
            }
        }
    }

}
