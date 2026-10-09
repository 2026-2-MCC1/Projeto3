using UnityEngine;
using UnityEngine.UI;

public class PlayerLivesUI : MonoBehaviour
{
    [SerializeField] private Image life1;
    [SerializeField] private Image life2;
    [SerializeField] private Image life3;

    private int lives = 3;
    public int CurrentLives => lives;

    private void OnEnable()
    {
        GameEvents.OnPlayerLifeLost += LoseLife;
    }
        
    private void OnDisable()
    {
        GameEvents.OnPlayerLifeLost -= LoseLife;
    }

    private void Start()
    {
        UpdateLives();
    }

    public void LoseLife()
    {
        if (lives <= 0)
            return;

        lives--;
        UpdateLives();
    }

    private void UpdateLives()
    {
        life1.enabled = lives >= 1;
        life2.enabled = lives >= 2;
        life3.enabled = lives >= 3;
    }
}