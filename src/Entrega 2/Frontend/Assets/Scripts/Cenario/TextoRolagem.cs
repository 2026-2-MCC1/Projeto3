using UnityEngine;
using UnityEngine.SceneManagement;

public class TextoRolagem : MonoBehaviour
{
    [Header("Configurações da Rolagem")]
    public float velocidade = 25f;
    public float tempoDePausa = 2f;
    public float tempoEsperaAposFim = 1.5f;

    private RectTransform rectTransform;
    private float cronometro = 0f;
    private bool textoAcabou = false;
    private CanvasGroup canvasGroup;
    private float alturaLimiteCalculada;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Adiciona ou obtém o CanvasGroup para o efeito de fade out
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Calcula a altura do texto + a altura da tela para garantir que ele suba COMPLETAMENTE
        float alturaTexto = rectTransform.rect.height;
        alturaLimiteCalculada = rectTransform.anchoredPosition.y + alturaTexto + Screen.height;
    }

    void Update()
    {
        if (textoAcabou) return;

        if (cronometro < tempoDePausa)
        {
            cronometro += Time.deltaTime;
            return;
        }

        Vector3 novaPosicao = rectTransform.anchoredPosition;
        novaPosicao.y += velocidade * Time.deltaTime;
        rectTransform.anchoredPosition = novaPosicao;

        // Só ativa a transição quando todo o texto ultrapassar a tela
        if (novaPosicao.y >= alturaLimiteCalculada)
        {
            textoAcabou = true;
            StartCoroutine(SumirEIniciarJogo());
        }
    }

    System.Collections.IEnumerator SumirEIniciarJogo()
    {
        float tempoDecorrido = 0f;
        float duracaoFade = 1f;

        while (tempoDecorrido < duracaoFade)
        {
            tempoDecorrido += Time.deltaTime;
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, tempoDecorrido / duracaoFade);
            }
            yield return null;
        }

        yield return new WaitForSeconds(tempoEsperaAposFim);
        SceneManager.LoadScene("Fase1");
    }
}