using System;

public enum Judgment { Perfect, Good, Miss }

public static class GameEvents
{
    // Entrada / julgamento
    public static event Action<int> OnLanePressed;          // pista 0..3
    public static event Action<Judgment> OnNoteJudged;      // Perfeito/Bom/Errou
    public static event Action<int, Judgment> OnLaneJudged; // pista + resultado (usado pelos efeitos)

    // Combate
    public static event Action<int, int> OnBossHealthChanged; // atual, máximo
    public static event Action<int, int> OnPlayerLifeLost;    // restantes, máximo
    public static event Action<int> OnBossDefeated;           // fase 1..3
    public static event Action<int> OnPhaseCompleted;         // fase 1..3
    public static event Action OnPlayerDefeated;

    // Pontuação: pontos totais da fase, combo atual, multiplicador
    public static event Action<int, int, int> OnScoreChanged;


    public static void LanePressed(int lane) => OnLanePressed?.Invoke(lane);
    public static void NoteJudged(Judgment j) => OnNoteJudged?.Invoke(j);
    public static void LaneJudged(int lane, Judgment j) => OnLaneJudged?.Invoke(lane, j);
    public static void BossHealthChanged(int cur, int max) => OnBossHealthChanged?.Invoke(cur, max);
    public static void PlayerLifeLost(int left, int max) => OnPlayerLifeLost?.Invoke(left, max);
    public static void BossDefeated(int phase) => OnBossDefeated?.Invoke(phase);
    public static void PhaseCompleted(int phase) => OnPhaseCompleted?.Invoke(phase);
    public static void PlayerDefeated() => OnPlayerDefeated?.Invoke();
    public static void ScoreChanged(int pontos, int combo, int mult) => OnScoreChanged?.Invoke(pontos, combo, mult);
}
