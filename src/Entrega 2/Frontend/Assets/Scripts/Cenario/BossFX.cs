using UnityEngine;

// Mantém o boss visível e parado: só pisca de cor ao levar dano.
// Ao ser derrotado, a imagem permanece no cenário sem desaparecer.
public class BossFX : MonoBehaviour
{
    SpriteRenderer sr;
    Color baseCor;
    float hit = 0f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        baseCor = sr != null ? sr.color : Color.white;
    }

    void OnEnable()
    {
        GameEvents.OnNoteJudged += AoJulgar;
    }

    void OnDisable()
    {
        GameEvents.OnNoteJudged -= AoJulgar;
    }

    void AoJulgar(Judgment j)
    {
        if (j == Judgment.Miss) return;
        hit = (j == Judgment.Perfect) ? 1f : 0.6f;
    }

    void Update()
    {
        if (sr == null) return;
        hit = Mathf.MoveTowards(hit, 0f, Time.deltaTime * 3.5f);
        sr.color = Color.Lerp(baseCor, new Color(1f, 0.55f, 0.55f, baseCor.a), hit * 0.6f);
    }
}
