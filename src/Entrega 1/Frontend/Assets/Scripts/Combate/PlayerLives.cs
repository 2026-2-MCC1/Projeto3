using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3;
    public bool debugKeys = false;   // D = perder vida (só para teste isolado)

    int currentLives;

    void OnEnable() { GameEvents.OnNoteJudged += HandleJudgment; }
    void OnDisable() { GameEvents.OnNoteJudged -= HandleJudgment; }

    void Start()
    {
        currentLives = maxLives;
        GameEvents.PlayerLifeLost(currentLives, maxLives); // atualiza o HUD no início
    }

    void HandleJudgment(Judgment j)
    {
        if (j == Judgment.Miss) LoseLife();
    }

    public void LoseLife()
    {
        if (currentLives <= 0 || GameStateManager.Current != GameState.Playing) return;

        currentLives--;
        GameEvents.PlayerLifeLost(currentLives, maxLives);

        if (currentLives <= 0) GameEvents.PlayerDefeated();
    }

    void Update()
    {
        if (debugKeys && Keyboard.current != null && Keyboard.current.dKey.wasPressedThisFrame)
            LoseLife();
    }
}