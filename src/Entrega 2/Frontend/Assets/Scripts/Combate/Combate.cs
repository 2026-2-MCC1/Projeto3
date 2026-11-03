using UnityEngine;

public class Combate : MonoBehaviour
{
    // Passo 1: criar o script que cuida do combate (vida do boss, vidas do jogador e pontos)
    // Passo 2: variaveis do jogo: vidas do jogador = 3, pontos por acerto = 10,
    //          dano que o boss leva por acerto = 10 e vida do boss = 100
    // Passo 3: guardar o que o script precisa lembrar: os pontos do jogador, se o jogo já acabou
    //          (ganhando ou perdendo) e a mensagem final
    // Passo 4: fazer o combate ficar de olho no HitZone, para saber toda vez que o jogador acerta ou erra.
    //          Sem isso o combate nunca ficaria sabendo e a vida e os pontos não mudariam.
    //          Quando o combate sair da cena, ele para de olhar, para não dar erro
    // Passo 5: quando o jogador errar: tirar 1 vida; se as vidas acabarem, o jogo acaba e ele perdeu
    // Passo 6: quando o jogador acertar: tirar vida do boss e dar pontos; se a vida do boss acabar, ele venceu
    // Passo 7: mostrar na tela a vida do boss, as vidas, os pontos e a mensagem final

    public int vidaBoss = 250;
    public int danoPorAcerto = 10;
    public int vidasJogador = 3;
    public int pontosJogador = 10;

    int pontos = 0;
    bool terminou = false;
    string mensagem = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        if (terminou == true)
        {
            return;
        }

        if (julgamento == Judgment.Miss)
        {
            vidasJogador--;

            if (vidasJogador <= 0)
            {
                terminou = true;
                // mensagem = "VOCE PERDEU!";
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
                // mensagem = "VOCE VENCEU!";
            }
        }
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

    // Texto com contorno preto, para ler por cima de qualquer fundo
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

        // Painel no canto superior esquerdo: fundo escuro + borda rosa
        float margem = u * 0.5f;
        float w = u * 8f;
        float h = u * 5.6f;
        Rect painel = new Rect(margem, margem, w, h);
        Caixa(new Rect(painel.x - 4, painel.y - 4, painel.width + 8, painel.height + 8), new Color(1f, 0.45f, 0.8f, 1f));
        Caixa(painel, new Color(0.1f, 0.03f, 0.18f, 0.92f));

        float x = painel.x + u * 0.5f;
        float iw = painel.width - u;
        float y = painel.y + u * 0.3f;

        // Vida do boss: texto + barra
        Texto(new Rect(x, y, iw, u * 1.1f), "BOSS   " + vidaBoss + " / " + vidaBossMax, estilo, new Color(1f, 0.9f, 0.3f), borda);
        y += u * 1.2f;
        Caixa(new Rect(x, y, iw, u * 0.7f), Color.black);
        float fracao = Mathf.Clamp01((float)vidaBoss / vidaBossMax);
        Color corBarra = fracao > 0.5f ? new Color(0.3f, 0.95f, 0.25f) : (fracao > 0.25f ? new Color(1f, 0.8f, 0.1f) : new Color(1f, 0.25f, 0.25f));
        Caixa(new Rect(x + 3, y + 3, (iw - 6) * fracao, u * 0.7f - 6), corBarra);
        y += u * 1.0f;

        // Vidas do jogador: quadradinhos
        Texto(new Rect(x, y, u * 3f, u * 1.1f), "VIDAS", estilo, Color.white, borda);
        for (int i = 0; i < vidasMax; i++)
        {
            float qx = x + u * 3.4f + i * u * 1.1f;
            Caixa(new Rect(qx - 2, y + u * 0.15f - 2, u * 0.8f + 4, u * 0.8f + 4), Color.black);
            Color c = i < vidasJogador ? new Color(1f, 0.25f, 0.5f) : new Color(0.35f, 0.35f, 0.4f);
            Caixa(new Rect(qx, y + u * 0.15f, u * 0.8f, u * 0.8f), c);
        }
        y += u * 1.2f;

        // Pontos
        Texto(new Rect(x, y, iw, u * 1.1f), "PONTOS   " + pontos, estilo, new Color(0.5f, 1f, 1f), borda);

        // Mensagem final no centro da tela
        if (mensagem != "")
        {
            estilo.fontSize = Mathf.RoundToInt(u * 2f);
            estilo.alignment = TextAnchor.MiddleCenter;
            float mw = u * 14f, mh = u * 3f;
            Rect caixa = new Rect((Screen.width - mw) / 2f, (Screen.height - mh) / 2f, mw, mh);
            Caixa(new Rect(caixa.x - 4, caixa.y - 4, caixa.width + 8, caixa.height + 8), new Color(1f, 0.45f, 0.8f, 1f));
            Caixa(caixa, new Color(0.1f, 0.03f, 0.18f, 0.95f));
            Texto(caixa, mensagem, estilo, new Color(1f, 0.9f, 0.3f), borda * 1.5f);
        }
    }
}
