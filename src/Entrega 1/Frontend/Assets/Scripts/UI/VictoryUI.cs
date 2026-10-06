using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    [SerializeField] private GameObject victoryPanel;

    private void OnEnable()
    {
        GameEvents.OnBossDefeated += ShowVictory;
    }

    private void OnDisable()
    {
        GameEvents.OnBossDefeated -= ShowVictory;
    }

    private void Start()
    {
        victoryPanel.SetActive(false);
    }

    private void ShowVictory(int fase)
    {
        victoryPanel.SetActive(true);
    }
}