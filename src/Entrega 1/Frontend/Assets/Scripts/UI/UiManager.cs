using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Feedback de julgamento")]
    [SerializeField] private TextMeshProUGUI feedbackText;

    private void Start()
    {
        feedbackText.text = "";
    }

    private void OnEnable()
    {
        GameEvents.OnNoteJudged += ShowJudgment;
    }

    private void OnDisable()
    {
        GameEvents.OnNoteJudged -= ShowJudgment;
    }

    private void ShowJudgment(Judgment judgment)
    {
        switch (judgment)
        {
            case Judgment.Perfect:
                feedbackText.text = "Perfeito!";
                break;

            case Judgment.Good:
                feedbackText.text = "Bom!";
                break;

            case Judgment.Miss:
                feedbackText.text = "Errou!";
                break;
        }
    }
}
