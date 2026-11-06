using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuResultados : MonoBehaviour
{
    // Avança automaticamente para a próxima cena na ordem do Build Settings
    public void AvancarProximaFase()
    {
        int cenaAtual = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(cenaAtual + 1);
    }

    // Para reiniciar a fase atual automaticamente
    public void JogarNovamente()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void VoltarMenu()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}