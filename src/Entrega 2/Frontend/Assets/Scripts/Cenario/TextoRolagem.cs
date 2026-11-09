using UnityEngine;
using UnityEngine.SceneManagement;

public class TextoRolagem : MonoBehaviour
{
    [Header("Configurações da Rolagem")]
    public float tempoDePausaInicial = 2f;      // Tempo parado no início antes de começar a rolar
    public float velocidadeRolagem = 120f;      // Velocidade de subida (podes ajustar no Inspector se estiver muito rápido ou lento)
    public float tempoDePausaFinal = 3f;        // Tempo parado no topo antes de mudar de cena

    [Header("Limite Fixo")]
    public float posicaoAlvoY = 5900f;          // Ponto exato em Y onde a história termina

    private RectTransform rectTransform;
    private float cronometroInicio = 0f;
    private float cronometroFim = 0f;
    private bool textoChegouAoFim = false;
    private bool textoAcabou = false;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (textoAcabou) return;

        // 1. Pausa inicial antes de começar a rolar
        if (cronometroInicio < tempoDePausaInicial)
        {
            cronometroInicio += Time.deltaTime;
            return;
        }

        // 2. Movimento de subida do texto
        if (!textoChegouAoFim)
        {
            Vector3 novaPosicao = rectTransform.anchoredPosition;
            novaPosicao.y += velocidadeRolagem * Time.deltaTime;
            rectTransform.anchoredPosition = novaPosicao;

            // Quando o texto atinge exatamente a posição Y 5900
            if (novaPosicao.y >= posicaoAlvoY)
            {
                novaPosicao.y = posicaoAlvoY; // Trava exatamente no limite
                rectTransform.anchoredPosition = novaPosicao;
                textoChegouAoFim = true;
            }
            return;
        }

        // 3. Pausa final no topo para o jogador ler a última frase antes de mudar de cena
        if (cronometroFim < tempoDePausaFinal)
        {
            cronometroFim += Time.deltaTime;
            return;
        }

        // 4. Inicia a primeira fase
        textoAcabou = true;
        GameSession.NovoJogo();
        SceneManager.LoadScene("Phase1");
    }
}