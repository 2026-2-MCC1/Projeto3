using UnityEngine;
using UnityEngine.SceneManagement;

// Estado do jogo que sobrevive entre as cenas (fase atual e pontuação).
public static class GameSession
{
    public const int TotalFases = 3;

    public const string CenaMenu = "MenuInicial";
    public const string CenaVitoria = "TelaVitoria";
    public const string CenaDerrota = "TelaDerrota";

    public static int FaseAtual = 1;
    public static int Pontos;        // pontos da fase em andamento
    public static int PontosTotal;   // soma das fases vencidas
    public static int Combo;
    public static int MelhorCombo;
    public static int Perfeitos;
    public static int Bons;
    public static int Erros;
    // Quando true, a fase atual terminou e o jogador permanece nesta mesma cena.
    public static bool FaseEncerrada;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetEstatico()
    {
        FaseAtual = 1; Pontos = 0; PontosTotal = 0; Combo = 0; MelhorCombo = 0;
        Perfeitos = 0; Bons = 0; Erros = 0; FaseEncerrada = false;
    }

    public static string NomeCenaFase(int fase)
    {
        return "Phase" + Mathf.Clamp(fase, 1, TotalFases);
    }

    // Descobre a fase pelo nome da cena ("Phase2" -> 2). Devolve 1 se não conseguir.
    public static int FaseDaCenaAtual()
    {
        string nome = SceneManager.GetActiveScene().name;
        for (int f = 1; f <= TotalFases; f++)
            if (nome == NomeCenaFase(f)) return f;
        return 1;
    }

    public static void NovoJogo()
    {
        FaseAtual = 1;
        Pontos = 0; PontosTotal = 0; Combo = 0; MelhorCombo = 0;
        Perfeitos = 0; Bons = 0; Erros = 0;
        FaseEncerrada = false;
    }

    public static void IniciarFase(int fase)
    {
        FaseAtual = Mathf.Clamp(fase, 1, TotalFases);
        Pontos = 0; Combo = 0; MelhorCombo = 0;
        Perfeitos = 0; Bons = 0; Erros = 0;
        FaseEncerrada = false;
    }

    // Mantido por compatibilidade com botões antigos. Não reinicia nem troca de cena.
    public static void CarregarFaseAtual()
    {
        EncerrarFase();
    }

    // Mantido por compatibilidade com botões antigos das telas de resultado.
    // O encerramento não troca de fase nem carrega telas de vitória/derrota.
    public static void IrParaProximaFaseOuFim()
    {
        EncerrarFase();
    }

    public static void EncerrarFase()
    {
        FaseEncerrada = true;
    }

    // ---------- pontuação ----------
    public static int Multiplicador
    {
        get { return Mathf.Clamp(1 + Combo / 10, 1, 4); }
    }

    // Registra um julgamento e devolve os pontos ganhos.
    public static int Registrar(Judgment j, int pontosBase)
    {
        if (j == Judgment.Miss)
        {
            Erros++;
            Combo = 0;
            return 0;
        }

        if (j == Judgment.Perfect) Perfeitos++; else Bons++;
        Combo++;
        if (Combo > MelhorCombo) MelhorCombo = Combo;

        int baseP = (j == Judgment.Perfect) ? pontosBase : pontosBase / 2;
        int ganho = baseP * Multiplicador;
        Pontos += ganho;
        return ganho;
    }

    public static void ConcluirFase()
    {
        PontosTotal += Pontos;
    }
}
