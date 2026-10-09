
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CandyCollectionManager : MonoBehaviour
{
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private CandyInfoUI candyInfoUI;

    private HashSet<CandyData> collectedCandies =
        new HashSet<CandyData>();

    public int TotalCollected => collectedCandies.Count;

    private void Start()
    {
        UpdateCounter();
    }

    public void RegisterCandy(CandyData candy)
    {
        if (candy == null)
            return;

        if (collectedCandies.Add(candy))
        {
            Debug.Log(
                "Bombons coletados: " +
                TotalCollected + "/12"
            );

            UpdateCounter();

            if (candyInfoUI != null)
            {
                candyInfoUI.ShowCandy(candy);
            }
        }
    }

    private void UpdateCounter()
    {
        if (counterText != null)
        {
            counterText.text =
                "Bombons: " + TotalCollected + "/12";
        }
    }
}
