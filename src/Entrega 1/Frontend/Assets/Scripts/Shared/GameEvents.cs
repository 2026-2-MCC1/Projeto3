using System;

public enum Judgment { Perfect, Good, Miss }

public static class GameEvents
{
    public static event Action<Judgment> OnNoteJudged;
    public static event Action<int> OnBossDefeated;
    public static event Action OnPlayerLifeLost;
    public static event Action<int> OnPhaseCompleted;

    public static void RaiseNoteJudged(Judgment j) => OnNoteJudged?.Invoke(j);
    public static void RaiseBossDefeated(int fase) => OnBossDefeated?.Invoke(fase);
    public static void RaisePlayerLifeLost() => OnPlayerLifeLost?.Invoke();
    public static void RaisePhaseCompleted(int fase) => OnPhaseCompleted?.Invoke(fase);
}