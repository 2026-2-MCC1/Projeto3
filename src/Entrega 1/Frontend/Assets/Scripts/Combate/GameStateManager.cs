using UnityEngine;

public enum GameState { Playing, Victory, Defeat, GameComplete }

public class GameStateManager : MonoBehaviour
{
    public static GameState Current = GameState.Playing;
    public GameState currentState;   // só para ver no Inspector

    void Awake() { Set(GameState.Playing); }

    void OnEnable()
    {
        GameEvents.OnBossDefeated += HandleBossDefeated;
        GameEvents.OnPlayerDefeated += HandlePlayerDefeated;
    }

    void OnDisable()
    {
        GameEvents.OnBossDefeated -= HandleBossDefeated;
        GameEvents.OnPlayerDefeated -= HandlePlayerDefeated;
    }

    void Set(GameState s) { Current = s; currentState = s; }

    void HandleBossDefeated(int phase)
    {
        Set(phase >= 3 ? GameState.GameComplete : GameState.Victory);
        GameEvents.PhaseCompleted(phase);
    }

    void HandlePlayerDefeated() { Set(GameState.Defeat); }
}
