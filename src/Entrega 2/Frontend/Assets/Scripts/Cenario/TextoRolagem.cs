using UnityEngine;
using UnityEngine.SceneManagement;

public class TextoRolagem : MonoBehaviour
{
    [Header("Configurações da Rolagem")]
    public float velocidade = 25f;
    public float tempoDePausa = 2f;

    private RectTransform rectTransform;
    private float cronometro = 0f;
    private bool textoAcabou = false;
    private float alturaLimiteCalculada;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        // Calcula a altura do texto + a altura da tela para garantir que ele suba completamente.
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

        // Quando a história termina, inicia a primeira fase sem tela de transição.
        if (novaPosicao.y >= alturaLimiteCalculada)
        {
            textoAcabou = true;
            GameSession.NovoJogo();
            SceneManager.LoadScene("Phase1");
        }
    }
}
