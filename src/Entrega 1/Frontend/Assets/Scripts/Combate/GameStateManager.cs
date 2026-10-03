using UnityEngine;

public enum GameState
{
    Playing,
    Victory,
    Defeat,
    GameComplete
}

public class GameStateManager : MonoBehaviour
{
    public GameState currentState;

    void Start()
    {
        currentState = GameState.Playing;
    }

    public void SetVictory()
    {
        currentState = GameState.Victory;

        Debug.Log("VICTORY!");
    }

    public void SetDefeat()
    {
        currentState = GameState.Defeat;

        Debug.Log("DEFEAT!");
    }

    public void SetGameComplete()
    {
        currentState = GameState.GameComplete;

        Debug.Log("GAME COMPLETE!");
    }
}