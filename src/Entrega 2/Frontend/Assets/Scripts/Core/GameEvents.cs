using System;

public enum Judgment { Perfect, Good, Miss }

public static class GameEvents
{
    // Pessoa 1 dispara
    public static event Action<int> OnLanePressed;          // pista 0..3
    public static event Action<Judgment> OnNoteJudged;      // Perfeito/Bom/Errou

    // Pessoa 2 dispara
    public static event Action<int, int> OnBossHealthChanged; // atual, máximo
    public static event Action<int, int> OnPlayerLifeLost;    // restantes, máximo
    public static event Action<int> OnBossDefeated;           // fase 1..3
    public static event Action<int> OnPhaseCompleted;         // fase 1..3
    public static event Action OnPlayerDefeated;

    public static void LanePressed(int lane) => OnLanePressed?.Invoke(lane);
    public static void NoteJudged(Judgment j) => OnNoteJudged?.Invoke(j);
    public static void BossHealthChanged(int cur, int max) => OnBossHealthChanged?.Invoke(cur, max);
    public static void PlayerLifeLost(int left, int max) => OnPlayerLifeLost?.Invoke(left, max);
    public static void BossDefeated(int phase) => OnBossDefeated?.Invoke(phase);
    public static void PhaseCompleted(int phase) => OnPhaseCompleted?.Invoke(phase);
    public static void PlayerDefeated() => OnPlayerDefeated?.Invoke();
}
