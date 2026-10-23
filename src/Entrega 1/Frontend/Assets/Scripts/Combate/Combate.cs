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

    public int vidaBoss = 100;
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
                mensagem = "VOCE PERDEU!";
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
            }
        }
    }

    void OnGUI()
    {
        int tamanho = Screen.height / 20;

        GUIStyle estilo = new GUIStyle(GUI.skin.label);
        estilo.fontSize = tamanho;
        estilo.normal.textColor = Color.yellow;

        GUI.Label(new Rect(20, 20, tamanho * 12, tamanho * 1.5f), "Vida do Boss: " + vidaBoss, estilo);
        GUI.Label(new Rect(20, 20 + tamanho * 1.5f, tamanho * 12, tamanho * 1.5f), "Vidas: " + vidasJogador, estilo);
        GUI.Label(new Rect(20, 20 + tamanho * 3, tamanho * 12, tamanho * 1.5f), "Pontos: " + pontos, estilo);
        GUI.Label(new Rect(20, 20 + tamanho * 4.5f, tamanho * 12, tamanho * 1.5f), mensagem, estilo);
    }
}