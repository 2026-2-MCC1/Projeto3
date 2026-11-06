using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class TextRolagem : MonoBehaviour
{
    public TextMeshProUGUI textoStory;
    public CanvasGroup fadeCanvasGroup;
    public float velocidade = 150f;
    public float distanciaParaSubir = 800f; // Quantos pixels o texto vai andar para cima a partir de onde o puseste
    public float tempoEsperaInicio = 3.0f;
    public float tempoEsperaAposFim = 1.5f;
    public float duracaoFade = 1.5f;
    public string nomeProximaCena = "Phase1";

    private RectTransform rectTransform;
    private float posYInicial;
    private float posYFinal;
    private bool aRolar = false;

    void Start()
    {
        if (textoStory == null)
            textoStory = GetComponent<TextMeshProUGUI>();

        rectTransform = textoStory.GetComponent<RectTransform>();

        // Guarda a posição exata onde o colocaste no Unity
        posYInicial = rectTransform.anchoredPosition.y;

        // Calcula o limite final automaticamente somando a distância à posição inicial
        posYFinal = posYInicial + distanciaParaSubir;

        // Garante que o ecrã começa transparente
        if (fadeCanvasGroup != null)
            fadeCanvasGroup.alpha = 0f;

        StartCoroutine(EsperarParaComecar());
    }

    IEnumerator EsperarParaComecar()
    {
        // Congela o texto na posição inicial durante 3 segundos
        yield return new WaitForSeconds(tempoEsperaInicio);

        // Liberta o movimento
        aRolar = true;
    }

    void Update()
    {
        if (!aRolar) return;

        // Move o texto para cima
        Vector2 novaPosicao = rectTransform.anchoredPosition;
        novaPosicao.y += velocidade * Time.deltaTime;
        rectTransform.anchoredPosition = novaPosicao;

        // Quando atingir o limite calculado, para e faz a transição
        if (novaPosicao.y >= posYFinal)
        {
            aRolar = false;
            StartCoroutine(TransicaoSuaveECarregar());
        }
    }

    IEnumerator TransicaoSuaveECarregar()
    {
        yield return new WaitForSeconds(tempoEsperaAposFim);

        if (fadeCanvasGroup != null)
        {
            float tempoPassado = 0f;
            while (tempoPassado < duracaoFade)
            {
                tempoPassado += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, tempoPassado / duracaoFade);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }

        PlayerPrefs.SetString("UltimaFaseJogada", nomeProximaCena);
        SceneManager.LoadScene(nomeProximaCena);
    }
}