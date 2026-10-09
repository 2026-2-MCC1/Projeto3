using UnityEngine;
using UnityEngine.SceneManagement;

public class PhaseManager : MonoBehaviour
{
    public static int CurrentPhase
    {
        get
        {
            string n = SceneManager.GetActiveScene().name;
            if (n.StartsWith("Phase") && int.TryParse(n.Substring(5), out int p)) return p;
            return 1; // SampleScene / testes
        }
    }

    // Chamado pelo botão "Próxima fase" da tela de vitória (Pessoa 3) ou pelo SceneFlowManager.
    public void LoadNextPhase()
    {
        int next = CurrentPhase + 1;
        if (next > 3) { Debug.Log("GAME COMPLETE! -> Caixa de Bombom"); return; }
        SceneManager.LoadScene("Phase" + next);
    }

    public void RestartPhase() { SceneManager.LoadScene("Phase" + CurrentPhase); }
}
