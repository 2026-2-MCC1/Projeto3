using TMPro;
using UnityEngine;
using UnityEngine.UI;

// HUD da fase montado por código: barra de vida, pontos, combo e avisos de acerto.
public class FaseHUD : MonoBehaviour
{
    public TMP_FontAsset fonte;
    public Font fonteTTF;
    public Sprite sprRound;
    public Sprite sprVinheta;
    public Sprite sprListras;

    static readonly Color[] Acentos =
    {
        new Color(1f, 0.38f, 0.72f),   // Fase 1 - rosa chiclete
        new Color(1f, 0.22f, 0.38f),   // Fase 2 - vermelho 7Belo
        new Color(0.30f, 0.90f, 1f)    // Fase 3 - ciano cósmico
    };

    static readonly string[] Titulos = { "BIG BIG BIGÃO", "7BELOS", "PLUTONITA" };

    int fase = 1;
    Color acento;

    Canvas canvas;
    RectTransform barraFill, barraGhost, comboBox, judgeRt;
    Image flashVermelho, brilhoBarra;
    TextMeshProUGUI txtPontos, txtCombo, txtMult, txtJulgamento, txtPct;

    float alvoPct = 1f, fillPct = 1f, ghostPct = 1f;
    float pontosMostrados, pontosAlvo;
    int combo = 0, mult = 1;
    float judgeT = 99f, comboPulso = 0f, flashA = 0f;
    Color judgeCor = Color.white;

    void Awake()
    {
        fase = GameSession.FaseDaCenaAtual();
        fonte = UIKit.FontePronta(fonte, fonteTTF);
        acento = Acentos[Mathf.Clamp(fase, 1, 3) - 1];
        Montar();
    }

    void OnEnable()
    {
        GameEvents.OnBossHealthChanged += AoMudarVidaBoss;
        GameEvents.OnScoreChanged += AoMudarPontos;
        GameEvents.OnNoteJudged += AoJulgar;
    }

    void OnDisable()
    {
        GameEvents.OnBossHealthChanged -= AoMudarVidaBoss;
        GameEvents.OnScoreChanged -= AoMudarPontos;
        GameEvents.OnNoteJudged -= AoJulgar;
    }

    // ------------------------------------------------------------ montagem
    void Montar()
    {
        canvas = UIKit.NovoCanvas("HUD_Fase", 50);
        canvas.transform.SetParent(transform, false);
        Transform raiz = canvas.transform;

        // vinheta de ambiente
        Image vin = UIKit.Img("Vinheta", raiz, sprVinheta, new Color(acento.r * 0.25f, acento.g * 0.1f, acento.b * 0.3f, 0.55f), false);
        UIKit.Esticar(vin.rectTransform);

        // flash vermelho ao errar
        flashVermelho = UIKit.Img("FlashErro", raiz, sprVinheta, new Color(1f, 0.05f, 0.1f, 0f), false);
        UIKit.Esticar(flashVermelho.rectTransform);

        // ---- topo: nome do boss + barra de vida
        RectTransform topo = UIKit.Rect("Topo", raiz);
        UIKit.Fixo(topo, new Vector2(0.5f, 1f), new Vector2(0f, -22f), new Vector2(900f, 150f));
        topo.pivot = new Vector2(0.5f, 1f);

        Image faixa = UIKit.Img("Faixa", topo, sprRound, new Color(0.08f, 0.02f, 0.14f, 0.82f), true);
        UIKit.Ancora(faixa.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), Vector2.zero, new Vector2(620f, 58f));
        TextMeshProUGUI nome = UIKit.Txt("NomeBoss", faixa.transform, "FASE " + fase + "  •  " + Titulos[Mathf.Clamp(fase, 1, 3) - 1], 34f, Color.white, fonte, TextAlignmentOptions.Center);
        UIKit.Esticar(nome.rectTransform);
        UIKit.Contorno(nome, new Color(0.2f, 0.02f, 0.2f), 0.25f);
        nome.color = Color.Lerp(Color.white, acento, 0.35f);

        Image moldura = UIKit.Img("BarraMoldura", topo, sprRound, new Color(0.08f, 0.02f, 0.14f, 0.92f), true);
        UIKit.Ancora(moldura.rectTransform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -66f), new Vector2(880f, 48f));

        Image fundo = UIKit.Img("BarraFundo", moldura.transform, sprRound, new Color(0.25f, 0.08f, 0.3f, 1f), true);
        UIKit.Ancora(fundo.rectTransform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        fundo.rectTransform.offsetMin = new Vector2(6f, 6f);
        fundo.rectTransform.offsetMax = new Vector2(-6f, -6f);

        Image ghost = UIKit.Img("BarraGhost", fundo.transform, sprRound, new Color(1f, 1f, 1f, 0.85f), true);
        barraGhost = ghost.rectTransform;
        PosBarra(barraGhost, 1f);

        Image fill = UIKit.Img("BarraVida", fundo.transform, sprRound, acento, true);
        barraFill = fill.rectTransform;
        PosBarra(barraFill, 1f);

        Image listras = UIKit.Img("BarraListras", barraFill, sprListras, new Color(1f, 1f, 1f, 0.9f), false);
        listras.type = Image.Type.Tiled;
        UIKit.Esticar(listras.rectTransform);
        Image brilho = UIKit.Img("BarraBrilho", barraFill, sprRound, new Color(1f, 1f, 1f, 0.28f), true);
        UIKit.Ancora(brilho.rectTransform, new Vector2(0f, 0.5f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        brilho.rectTransform.offsetMin = new Vector2(6f, 0f);
        brilho.rectTransform.offsetMax = new Vector2(-6f, -3f);
        brilhoBarra = brilho;

        txtPct = UIKit.Txt("Pct", moldura.transform, "", 28f, Color.white, fonte, TextAlignmentOptions.Center);
        UIKit.Esticar(txtPct.rectTransform);
        UIKit.Contorno(txtPct, new Color(0.05f, 0f, 0.1f), 0.45f);

        // ---- pontos (esquerda)
        RectTransform pontos = UIKit.Rect("Pontos", raiz);
        UIKit.Fixo(pontos, new Vector2(0f, 1f), new Vector2(34f, -26f), new Vector2(380f, 120f));
        pontos.pivot = new Vector2(0f, 1f);
        Image pf = UIKit.Img("Fundo", pontos, sprRound, new Color(0.08f, 0.02f, 0.14f, 0.7f), true);
        UIKit.Esticar(pf.rectTransform);
        TextMeshProUGUI pl = UIKit.Txt("Rotulo", pontos, "PONTOS", 24f, acento, fonte, TextAlignmentOptions.TopLeft);
        UIKit.Ancora(pl.rectTransform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        pl.rectTransform.offsetMin = new Vector2(24f, 0f);
        pl.rectTransform.offsetMax = new Vector2(-10f, -12f);
        txtPontos = UIKit.Txt("Valor", pontos, "0", 62f, Color.white, fonte, TextAlignmentOptions.BottomLeft);
        UIKit.Ancora(txtPontos.rectTransform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        txtPontos.rectTransform.offsetMin = new Vector2(24f, 8f);
        txtPontos.rectTransform.offsetMax = new Vector2(-10f, 0f);
        UIKit.Contorno(txtPontos, new Color(0.15f, 0f, 0.2f), 0.25f);

        // ---- combo (direita)
        comboBox = UIKit.Rect("Combo", raiz);
        UIKit.Fixo(comboBox, new Vector2(1f, 1f), new Vector2(-34f, -26f), new Vector2(320f, 120f));
        comboBox.pivot = new Vector2(1f, 1f);
        Image cf = UIKit.Img("Fundo", comboBox, sprRound, new Color(0.08f, 0.02f, 0.14f, 0.7f), true);
        UIKit.Esticar(cf.rectTransform);
        TextMeshProUGUI cl = UIKit.Txt("Rotulo", comboBox, "COMBO", 24f, acento, fonte, TextAlignmentOptions.TopRight);
        UIKit.Ancora(cl.rectTransform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        cl.rectTransform.offsetMin = new Vector2(10f, 0f);
        cl.rectTransform.offsetMax = new Vector2(-24f, -12f);
        txtCombo = UIKit.Txt("Valor", comboBox, "0", 62f, Color.white, fonte, TextAlignmentOptions.BottomRight);
        UIKit.Ancora(txtCombo.rectTransform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        txtCombo.rectTransform.offsetMin = new Vector2(10f, 8f);
        txtCombo.rectTransform.offsetMax = new Vector2(-24f, 0f);
        UIKit.Contorno(txtCombo, new Color(0.15f, 0f, 0.2f), 0.25f);
        txtMult = UIKit.Txt("Mult", comboBox, "x1", 38f, new Color(1f, 0.9f, 0.3f), fonte, TextAlignmentOptions.BottomLeft);
        UIKit.Ancora(txtMult.rectTransform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
        txtMult.rectTransform.offsetMin = new Vector2(24f, 12f);
        txtMult.rectTransform.offsetMax = new Vector2(0f, 0f);
        UIKit.Contorno(txtMult, new Color(0.3f, 0.1f, 0f), 0.3f);

        // ---- aviso de julgamento
        txtJulgamento = UIKit.Txt("Julgamento", raiz, "", 52f, Color.white, fonte, TextAlignmentOptions.Center);
        judgeRt = txtJulgamento.rectTransform;
        UIKit.Fixo(judgeRt, new Vector2(0.88f, 0.62f), Vector2.zero, new Vector2(320f, 82f));
        UIKit.Contorno(txtJulgamento, new Color(0.1f, 0f, 0.15f), 0.5f);
        txtJulgamento.alpha = 0f;

    }

    static void PosBarra(RectTransform r, float pct)
    {
        r.anchorMin = new Vector2(0f, 0f);
        r.anchorMax = new Vector2(Mathf.Clamp01(pct), 1f);
        r.offsetMin = Vector2.zero;
        r.offsetMax = Vector2.zero;
    }

    // ------------------------------------------------------------ eventos
    void AoMudarVidaBoss(int atual, int max)
    {
        alvoPct = max > 0 ? Mathf.Clamp01((float)atual / max) : 0f;
        txtPct.text = atual + " / " + max;
    }

    void AoMudarPontos(int pts, int c, int m)
    {
        pontosAlvo = pts;
        if (c > combo) comboPulso = 1f;
        combo = c;
        mult = m;
        txtCombo.text = c.ToString();
        txtMult.text = "x" + m;
    }

    void AoJulgar(Judgment j)
    {
        judgeT = 0f;
        switch (j)
        {
            case Judgment.Perfect:
                txtJulgamento.text = "PERFEITO!";
                judgeCor = new Color(1f, 0.95f, 0.15f);
                txtJulgamento.outlineColor = new Color(0.35f, 0.12f, 0f);
                break;
            case Judgment.Good:
                txtJulgamento.text = "BOM!";
                judgeCor = new Color(0.35f, 1f, 0.55f);
                txtJulgamento.outlineColor = new Color(0f, 0.25f, 0.1f);
                break;
            default:
                txtJulgamento.text = "ERROU!";
                judgeCor = new Color(1f, 0.25f, 0.25f);
                txtJulgamento.outlineColor = new Color(0.25f, 0f, 0f);
                flashA = 0.4f;
                break;
        }
        txtJulgamento.outlineWidth = 0.5f;
        txtJulgamento.color = judgeCor;
    }

    // ------------------------------------------------------------ animação
    void Update()
    {
        float dt = Time.deltaTime;
        // barra de vida
        fillPct = Mathf.MoveTowards(fillPct, alvoPct, dt * 1.6f);
        if (ghostPct > fillPct) ghostPct = Mathf.MoveTowards(ghostPct, fillPct, dt * 0.35f);
        else ghostPct = fillPct;
        PosBarra(barraFill, fillPct);
        PosBarra(barraGhost, ghostPct);
        float pulso = alvoPct < 0.25f ? 0.5f + 0.5f * Mathf.Sin(Time.time * 10f) : 0f;
        brilhoBarra.color = new Color(1f, 1f, 1f, 0.28f + pulso * 0.25f);

        // pontos suaves
        pontosMostrados = Mathf.MoveTowards(pontosMostrados, pontosAlvo, Mathf.Max(60f, (pontosAlvo - pontosMostrados) * 6f) * dt);
        txtPontos.text = Mathf.RoundToInt(pontosMostrados).ToString("N0");

        // combo pulsando
        comboPulso = Mathf.MoveTowards(comboPulso, 0f, dt * 5f);
        float cs = 1f + comboPulso * 0.18f;
        comboBox.localScale = new Vector3(cs, cs, 1f);
        txtMult.color = mult >= 4 ? new Color(1f, 0.45f, 0.2f) : new Color(1f, 0.9f, 0.3f);

        // julgamento
        if (judgeT < 1f)
        {
            judgeT += dt;
            float k = Mathf.Clamp01(judgeT / 0.7f);
            float pop = k < 0.15f ? Mathf.Lerp(0.5f, 1.25f, k / 0.15f) : Mathf.Lerp(1.25f, 1f, Mathf.Clamp01((k - 0.15f) / 0.15f));
            judgeRt.localScale = new Vector3(pop, pop, 1f);
            txtJulgamento.alpha = 1f - Mathf.Clamp01((k - 0.55f) / 0.45f);
        }
        else txtJulgamento.alpha = 0f;

        // flash de erro
        flashA = Mathf.MoveTowards(flashA, 0f, dt * 2.2f);
        flashVermelho.color = new Color(1f, 0.05f, 0.1f, flashA);

    }
}
