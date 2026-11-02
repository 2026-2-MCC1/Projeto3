using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DesafioMatematico : MonoBehaviour
{
    [Header("Botões das opções")]
    public Button BotaoA; // 68
    public Button BotaoB; // 70
    public Button BotaoC; // 76
    public Button BotaoD; // 82

    [Header("Botão de avançar")]
    public Button BotaoAvancar;

    [Header("Texto de feedback")]
    public TextMeshProUGUI TextoFeedback;

    [Header("Nome da próxima cena")]
    public string NomeCenaDoces = "CenaDoces";

    private int RespostaCorreta = 76;

    private Color CorVerde = new Color(0.2f, 0.8f, 0.2f);
    private Color CorVermelha = new Color(0.9f, 0.2f, 0.2f);
    private Color CorOriginal = new Color(0.91f, 0.86f, 0.78f); // bege da arte
    void Start()
    {
        BotaoAvancar.gameObject.SetActive(false);
        TextoFeedback.text = "";

        BotaoA.onClick.AddListener(() => Responder(68, BotaoA));
        BotaoB.onClick.AddListener(() => Responder(70, BotaoB));
        BotaoC.onClick.AddListener(() => Responder(76, BotaoC));
        BotaoD.onClick.AddListener(() => Responder(82, BotaoD));

        BotaoAvancar.onClick.AddListener(Avancar);
    }

    void Responder(int opcao, Button botaoClicado)
    {
        // Reseta cor de todos os botões
        BotaoA.GetComponent<Image>().color = CorOriginal;
        BotaoB.GetComponent<Image>().color = CorOriginal;
        BotaoC.GetComponent<Image>().color = CorOriginal;
        BotaoD.GetComponent<Image>().color = CorOriginal;

        if (opcao == RespostaCorreta)
        {
            botaoClicado.GetComponent<Image>().color = CorVerde;
            TextoFeedback.text = "Correto! O código é 76!";
            TextoFeedback.color = CorVerde;
            BotaoAvancar.gameObject.SetActive(true);
        }
        else
        {
            botaoClicado.GetComponent<Image>().color = CorVermelha;
            TextoFeedback.text = "Errado! Tente novamente.";
            TextoFeedback.color = CorVermelha;
            BotaoAvancar.gameObject.SetActive(false);
        }
    }

    void Avancar()
    {
        SceneManager.LoadScene(NomeCenaDoces);
    }
}