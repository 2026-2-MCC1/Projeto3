using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    public int currentPhase = 1;

    public void LoadNextPhase()
    {
        currentPhase++;

        if (currentPhase > 3)
        {
            CompleteGame();
            return;
        }

        SceneManager.LoadScene("Phase" + currentPhase);
    }

    private void CompleteGame()
    {
        Debug.Log("GAME COMPLETE!");
    }
}