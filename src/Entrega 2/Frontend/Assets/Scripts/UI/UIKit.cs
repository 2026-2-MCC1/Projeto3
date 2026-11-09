using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Pequenas funções para montar interface por código (usadas pelo HUD e pela Caixa de Doces).
public static class UIKit
{
    static TMP_FontAsset fonteDinamica;

    // Cria (uma vez) uma fonte TMP dinâmica a partir do TTF: assim todos os acentos (Ã, Ç, É...) aparecem.
    // Se algum símbolo não existir no TTF, usa a fonte padrão do TMP como reserva.
    public static TMP_FontAsset FontePronta(TMP_FontAsset original, Font ttf)
    {
        if (fonteDinamica != null) return fonteDinamica;
        if (ttf == null) return original;
        TMP_FontAsset f = TMP_FontAsset.CreateFontAsset(ttf);
        if (f == null) return original;
        if (TMP_Settings.defaultFontAsset != null)
        {
            if (f.fallbackFontAssetTable == null) f.fallbackFontAssetTable = new System.Collections.Generic.List<TMP_FontAsset>();
            f.fallbackFontAssetTable.Add(TMP_Settings.defaultFontAsset);
        }
        fonteDinamica = f;
        return f;
    }

    public static Canvas NovoCanvas(string nome, int ordem)
    {
        GameObject go = new GameObject(nome);
        Canvas c = go.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        c.sortingOrder = ordem;
        CanvasScaler cs = go.AddComponent<CanvasScaler>();
        cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        cs.referenceResolution = new Vector2(1920f, 1080f);
        cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        cs.matchWidthOrHeight = 0.5f;
        go.AddComponent<GraphicRaycaster>();
        return c;
    }

    public static RectTransform Rect(string nome, Transform pai)
    {
        GameObject go = new GameObject(nome, typeof(RectTransform));
        go.transform.SetParent(pai, false);
        return go.GetComponent<RectTransform>();
    }

    public static void Ancora(RectTransform r, Vector2 ancMin, Vector2 ancMax, Vector2 pivo, Vector2 pos, Vector2 tam)
    {
        r.anchorMin = ancMin;
        r.anchorMax = ancMax;
        r.pivot = pivo;
        r.anchoredPosition = pos;
        r.sizeDelta = tam;
    }

    // Âncora única (mesmo ponto) com pivô igual à âncora.
    public static void Fixo(RectTransform r, Vector2 ancora, Vector2 pos, Vector2 tam)
    {
        Ancora(r, ancora, ancora, ancora, pos, tam);
    }

    public static void Esticar(RectTransform r)
    {
        r.anchorMin = Vector2.zero;
        r.anchorMax = Vector2.one;
        r.offsetMin = Vector2.zero;
        r.offsetMax = Vector2.zero;
    }

    public static Image Img(string nome, Transform pai, Sprite s, Color cor, bool fatiado)
    {
        RectTransform r = Rect(nome, pai);
        Image im = r.gameObject.AddComponent<Image>();
        im.sprite = s;
        im.color = cor;
        im.type = (fatiado && s != null) ? Image.Type.Sliced : Image.Type.Simple;
        im.raycastTarget = false;
        return im;
    }

    public static TextMeshProUGUI Txt(string nome, Transform pai, string texto, float tamanho, Color cor, TMP_FontAsset fonte, TextAlignmentOptions alinhar)
    {
        RectTransform r = Rect(nome, pai);
        TextMeshProUGUI t = r.gameObject.AddComponent<TextMeshProUGUI>();
        if (fonte != null) t.font = fonte;
        t.text = texto;
        t.fontSize = tamanho;
        t.color = cor;
        t.alignment = alinhar;
        t.textWrappingMode = TextWrappingModes.NoWrap;
        t.raycastTarget = false;
        return t;
    }

    public static void Contorno(TextMeshProUGUI t, Color cor, float largura)
    {
        t.outlineColor = cor;
        t.outlineWidth = largura;
    }
}
