
using UnityEngine;
using TMPro;

public class CandyInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TMP_Text infoText;

    private void Start()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    public void ShowCandy(CandyData candy)
    {
        if (candy == null)
            return;

        if (infoPanel != null)
            infoPanel.SetActive(true);

        if (infoText != null)
        {
            infoText.text =
                candy.nome + "\n\n" +
                candy.curiosidade;
        }
    }

    public void HidePanel()
    {
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }
}
