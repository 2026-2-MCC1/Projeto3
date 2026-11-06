using UnityEngine;
using UnityEngine.SceneManagement;

public class Combate : MonoBehaviour
{
    public int vidaBoss = 250;
    public int danoPorAcerto = 10;
    public int vidasJogador = 3;
    public int pontosJogador = 10;

    [Header("Cenas de Transição")]
    public string nomeCenaDerrota = "TelaDerrota";
    public string nomeCenaVitoria = "TelaVitoria";

    int pontos = 0;
    bool terminou = false;
    string mensagem = "";

    void OnEnable()
    {
        GameEvents.OnNoteJudged += AoJulgarNota;
    }

    void OnDisable()
    {
        GameEvents.OnNoteJudged -= AoJulgarNota;
    }

    void AoJulgarNota(Judgment julgamento)
    {
        if (terminou == true) return;

        if (julgamento == Judgment.Miss)
        {
            vidasJogador--;
            if (vidasJogador <= 0)
            {
                vidasJogador = 0;
                terminou = true;
                mensagem = "VOCE PERDEU!";
                StartCoroutine(IrParaDerrota());
            }
        }
        else
        {
            vidaBoss -= danoPorAcerto;
            pontos += pontosJogador;
            if (vidaBoss <= 0)
            {
                vidaBoss = 0;
                terminou = true;
                mensagem = "VOCE VENCEU!";
                StartCoroutine(IrParaVitoria());
            }
        }
    }

    System.Collections.IEnumerator IrParaDerrota()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(nomeCenaDerrota);
    }

    System.Collections.IEnumerator IrParaVitoria()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(nomeCenaVitoria);
    }

    // ---------- Placar (HUD) ----------
    int vidaBossMax;
    int vidasMax;
    Texture2D pixel;

    void Caixa(Rect r, Color cor)
    {
        Color antes = GUI.color;
        GUI.color = cor;
        GUI.DrawTexture(r, pixel);
        GUI.color = antes;
    }

    void Texto(Rect r, string texto, GUIStyle estilo, Color cor, float borda)
    {
        Color original = estilo.normal.textColor;
        estilo.normal.textColor = Color.black;
        GUI.Label(new Rect(r.x - borda, r.y, r.width, r.height), texto, estilo);
        GUI.Label(new Rect(r.x + borda, r.y, r.width, r.height), texto, estilo);
        GUI.Label(new Rect(r.x, r.y - borda, r.width, r.height), texto, estilo);
        GUI.Label(new Rect(r.x, r.y + borda, r.width, r.height), texto, estilo);
        estilo.normal.textColor = cor;
        GUI.Label(r, texto, estilo);
        estilo.normal.textColor = original;
    }

    void OnGUI()
    {
        if (pixel == null)
        {
            pixel = new Texture2D(1, 1);
            pixel.SetPixel(0, 0, Color.white);
            pixel.Apply();
        }
        if (vidaBossMax == 0) vidaBossMax = Mathf.Max(1, vidaBoss);
        if (vidasMax == 0) vidasMax = Mathf.Max(1, vidasJogador);

        float u = Screen.height / 20f;
        float borda = Mathf.Max(1f, u * 0.06f);

        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.fontSize = Mathf.RoundToInt(u * 0.8f);
        estilo.fontStyle = FontStyle.Bold;
        estilo.alignment = TextAnchor.MiddleLeft;
        estilo.wordWrap = false;

        // Barra do Boss
        float bossW = u * 12f;
        float bossH = u * 2.2f;
        float bossX = (Screen.width - bossW) / 2f;
        float bossY = (Screen.height / 2f) - (u * 4f);

        Rect painelBoss = new Rect(bossX, bossY, bossW, bossH);
        Caixa(new Rect(painelBoss.x - 4, painelBoss.y - 4, painelBoss.width + 8, painelBoss.height + 8), new Color(1f, 0.45f, 0.8f, 1f));
        Caixa(painelBoss, new Color(0.1f, 0.03f, 0.18f, 0.92f));

        float bx = painelBoss.x + u * 0.6f;
        float biw = painelBoss.width - (u * 1.2f);
        float by = painelBoss.y + u * 0.25f;

        estilo.alignment = TextAnchor.MiddleCenter;
        Texto(new Rect(bx, by, biw, u * 0.9f), "BOSS   " + vidaBoss + " / " + vidaBossMax, estilo, new Color(1f, 0.9f, 0.3f), borda);
        by += u * 0.95f;

        Caixa(new Rect(bx, by, biw, u * 0.6f), Color.black);
        float fracao = Mathf.Clamp01((float)vidaBoss / vidaBossMax);
        Color corBarra = fracao > 0.5f ? new Color(0.3f, 0.95f, 0.25f) : (fracao > 0.25f ? new Color(1f, 0.8f, 0.1f) : new Color(1f, 0.25f, 0.25f));
        Caixa(new Rect(bx + 2, by + 2, (biw - 4) * fracao, u * 0.6f - 4), corBarra);

        estilo.alignment = TextAnchor.MiddleLeft;

        // Vidas e Pontuação
        float margem = u * 0.5f;
        float painelDirW = u * 8f;
        float painelDirH = u * 4.2f;
        float painelDirX = Screen.width - painelDirW - margem;
        float painelDirY = margem;

        Rect painelInfo = new Rect(painelDirX, painelDirY, painelDirW, painelDirH);
        Caixa(new Rect(painelInfo.x - 4, painelInfo.y - 4, painelInfo.width + 8, painelInfo.height + 8), new Color(1f, 0.45f, 0.8f, 1f));
        Caixa(painelInfo, new Color(0.1f, 0.03f, 0.18f, 0.92f));

        float ix = painelInfo.x + u * 0.5f;
        float iwi = painelInfo.width - u;
        float iy = painelInfo.y + u * 0.3f;

        Texto(new Rect(ix, iy, u * 2.5f, u * 1.1f), "VIDAS", estilo, Color.white, borda);
        for (int i = 0; i < vidasMax; i++)
        {
            float qx = ix + u * 2.6f + (i * u * 0.9f);
            Caixa(new Rect(qx - 2, iy + u * 0.15f - 2, u * 0.8f + 4, u * 0.8f + 4), Color.black);
            Color c = i < vidasJogador ? new Color(1f, 0.25f, 0.5f) : new Color(0.35f, 0.35f, 0.4f);
            Caixa(new Rect(qx, iy + u * 0.15f, u * 0.8f, u * 0.8f), c);
        }
        iy += u * 1.2f;

        Texto(new Rect(ix, iy, iwi, u * 1.1f), "PONTOS   " + pontos, estilo, new Color(0.5f, 1f, 1f), borda);

        // Mensagem Final
        if (mensagem != "")
        {
            estilo.fontSize = Mathf.RoundToInt(u * 2f);
            estilo.alignment = TextAnchor.MiddleCenter;
            float mw = u * 14f;
            float mh = u * 3f;
            Rect caixaMsg = new Rect((Screen.width - mw) / 2f, (Screen.height - mh) / 2f, mw, mh);

            Caixa(new Rect(caixaMsg.x - 4, caixaMsg.y - 4, caixaMsg.width + 8, caixaMsg.height + 8), new Color(1f, 0.45f, 0.8f, 1f));
            Caixa(caixaMsg, new Color(0.1f, 0.03f, 0.18f, 0.95f));
            Texto(caixaMsg, mensagem, estilo, new Color(1f, 0.9f, 0.3f), borda * 1.5f);
        }
    }
}