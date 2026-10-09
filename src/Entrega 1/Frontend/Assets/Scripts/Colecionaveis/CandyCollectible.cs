
using UnityEngine;
using UnityEngine.InputSystem;

public class CandyCollectible : MonoBehaviour
{
    [SerializeField] private CandyData candyData;
    [SerializeField] private CandyCollectionManager collectionManager;

    private bool collected = false;

    public CandyData Data => candyData;

    public void Collect()
    {
        if (collected)
            return;

        if (candyData == null || collectionManager == null)
        {
            Debug.LogWarning(
                "Configure CandyData e CandyCollectionManager no Inspector.",
                this
            );
            return;
        }

        collected = true;

        collectionManager.RegisterCandy(candyData);

        Debug.Log("Bombom coletado: " + candyData.nome);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            Collect();
        }
    }
}
