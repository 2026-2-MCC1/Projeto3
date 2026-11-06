using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public CanvasGroup painelFade; // Arrasta o PainelFadeIn para aqui
    public float duracaoFade = 1.5f; // Tempo que demora a clarear

    void Start()
    {
        if (painelFade == null)
        {
            painelFade = GetComponent<CanvasGroup>();
        }

        if (painelFade != null)
        {
            painelFade.alpha = 1f; // Começa totalmente preto
            StartCoroutine(ExecutarFadeIn());
        }
    }

    System.Collections.IEnumerator ExecutarFadeIn()
    {
        float tempoDecorrido = 0f;

        while (tempoDecorrido < duracaoFade)
        {
            tempoDecorrido += Time.deltaTime;
            painelFade.alpha = Mathf.Lerp(1f, 0f, tempoDecorrido / duracaoFade);
            yield return null;
        }

        painelFade.alpha = 0f; // Fica totalmente transparente
        painelFade.gameObject.SetActive(false); // Desativa o painel para não bloquear cliques
    }
}