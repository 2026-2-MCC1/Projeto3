using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Painéis do Menu")]
    public GameObject painelAjustes;
    public GameObject fundoEscuro;

    public void JogarJogo()
    {
        GameSession.NovoJogo();
        SceneManager.LoadScene("Historia");
    }

    public void AbrirAjustes()
    {
        if (painelAjustes != null) painelAjustes.SetActive(true);
        if (fundoEscuro != null) fundoEscuro.SetActive(true);
    }

    public void FecharAjustes()
    {
        if (painelAjustes != null) painelAjustes.SetActive(false);
        if (fundoEscuro != null) fundoEscuro.SetActive(false);
    }

    public void SairJogo()
    {
        Debug.Log("O jogo foi fechado.");
        Application.Quit();
    }
}
