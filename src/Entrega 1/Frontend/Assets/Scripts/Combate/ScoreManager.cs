using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public int combo;

    void OnEnable() { GameEvents.OnNoteJudged += HandleJudgment; }
    void OnDisable() { GameEvents.OnNoteJudged -= HandleJudgment; }

    void HandleJudgment(Judgment j)
    {
        switch (j)
        {
            case Judgment.Perfect: combo++; score += 100 * GetMultiplier(); break;
            case Judgment.Good: combo++; score += 50 * GetMultiplier(); break;
            case Judgment.Miss: combo = 0; break;
        }
    }

    int GetMultiplier()
    {
        if (combo >= 30) return 4;
        if (combo >= 20) return 3;
        if (combo >= 10) return 2;
        return 1;
    }
}
