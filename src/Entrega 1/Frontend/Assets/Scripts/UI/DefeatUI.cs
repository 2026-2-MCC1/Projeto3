
using UnityEngine;

public class DefeatUI : MonoBehaviour
{
    [SerializeField] private GameObject defeatPanel;
    private PlayerLivesUI playerLives;

    private void OnEnable()
    {
        GameEvents.OnPlayerLifeLost += CheckDefeat;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayerLifeLost -= CheckDefeat;
    }

    private void Start()
    {
        playerLives = FindFirstObjectByType<PlayerLivesUI>();

        if (defeatPanel != null)
            defeatPanel.SetActive(false);
    }

    private void CheckDefeat()
    {
        if (playerLives == null)
            return;

        if (playerLives.CurrentLives <= 0)
        {
            defeatPanel.SetActive(true);
        }
    }
}
