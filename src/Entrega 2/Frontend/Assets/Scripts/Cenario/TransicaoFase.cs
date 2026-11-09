using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TransicaoFase : MonoBehaviour
{
    void OnEnable()
    {
        GameEvents.OnBossDefeated += AoDerrotarBoss;
        GameEvents.OnPlayerDefeated += AoDerrotarJogador;
    }

    void OnDisable()
    {
        GameEvents.OnBossDefeated -= AoDerrotarBoss;
        GameEvents.OnPlayerDefeated -= AoDerrotarJogador;
    }

    void AoDerrotarBoss(int fase)
    {
        string cenaDestino = "Recompensas1";

        if (fase == 1) cenaDestino = "Recompensas1";
        else if (fase == 2) cenaDestino = "Recompensas2";
        else if (fase == 3) cenaDestino = "Recompensas3";

        StartCoroutine(MudarCenaComAtraso(cenaDestino, 3f));
    }

    void AoDerrotarJogador()
    {
        // Descobre em qual fase o jogador está pelo nome da cena atual
        string cenaAtual = SceneManager.GetActiveScene().name;
        string cenaDerrota = "TelaDerrota1"; // Padrão

        if (cenaAtual == "Phase1")
        {
            cenaDerrota = "TelaDerrota1";
        }
        else if (cenaAtual == "Phase2")
        {
            cenaDerrota = "TelaDerrota2";
        }
        else if (cenaAtual == "Phase3")
        {
            cenaDerrota = "TelaDerrota3";
        }

        StartCoroutine(MudarCenaComAtraso(cenaDerrota, 3f));
    }

    IEnumerator MudarCenaComAtraso(string nomeCena, float segundos)
    {
        yield return new WaitForSeconds(segundos);
        SceneManager.LoadScene(nomeCena);
    }
}