using UnityEngine;
using UnityEngine.UI;

public class VidasHUD : MonoBehaviour
{
    // Passo 1: guardar a imagem da tela que mostra as vidas e as 4 figuras
    //          (3 vidas cheias, 2, 1 e nenhuma)
    // Passo 2: ficar de olho no GameEvents para saber toda vez que o jogador perde uma vida.
    //          Quando o HUD sair da cena, ele para de olhar, para nao dar erro
    // Passo 3: trocar a figura de acordo com as vidas que sobraram

    public Image imagemVidas;
    public Sprite vidas3;
    public Sprite vidas2;
    public Sprite vidas1;
    public Sprite vidas0;

    void OnEnable()
    {
        GameEvents.OnPlayerLifeLost += AoMudarVidas;
        Mostrar(3);
    }

    void OnDisable()
    {
        GameEvents.OnPlayerLifeLost -= AoMudarVidas;
    }

    void AoMudarVidas(int restantes, int maximo)
    {
        Mostrar(restantes);
    }

    void Mostrar(int restantes)
    {
        if (imagemVidas == null)
        {
            return;
        }

        if (restantes >= 3)
        {
            imagemVidas.sprite = vidas3;
        }
        else if (restantes == 2)
        {
            imagemVidas.sprite = vidas2;
        }
        else if (restantes == 1)
        {
            imagemVidas.sprite = vidas1;
        }
        else
        {
            imagemVidas.sprite = vidas0;
        }
    }
}
