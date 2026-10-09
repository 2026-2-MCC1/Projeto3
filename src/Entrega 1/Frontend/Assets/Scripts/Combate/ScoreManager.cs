using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score;
    public int combo;

    public void RegisterPerfect()
    {
        combo++;

        int multiplier = GetMultiplier();

        score += 100 * multiplier;

        LogScore();
    }

    public void RegisterGood()
    {
        combo++;

        int multiplier = GetMultiplier();

        score += 50 * multiplier;

        LogScore();
    }

    public void RegisterMiss()
    {
        combo = 0;

        LogScore();
    }

    private int GetMultiplier()
    {
        if (combo >= 30)
            return 4;

        if (combo >= 20)
            return 3;

        if (combo >= 10)
            return 2;

        return 1;
    }

    private void LogScore()
    {
        Debug.Log(
            "Score: " + score +
            " | Combo: " + combo +
            " | Multiplier: x" + GetMultiplier()
        );
    }
}