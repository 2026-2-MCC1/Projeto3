using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLives : MonoBehaviour
{
    public int maxLives = 3;

    private int currentLives;

    void Start()
    {
        currentLives = maxLives;

        Debug.Log("Lives: " + currentLives);
    }

    public void LoseLife()
    {
        currentLives--;

        Debug.Log("Life lost!");
        Debug.Log("Remaining lives: " + currentLives);

        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER!");
    }

    void Update()
    {
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            LoseLife();
        }
    }
}