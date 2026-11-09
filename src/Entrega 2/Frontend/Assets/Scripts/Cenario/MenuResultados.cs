using UnityEngine;

public class MenuResultados : MonoBehaviour
{
    // Vai para a próxima fase (ou volta ao menu depois da última)
    public void AvancarProximaFase()
    {
        GameSession.IrParaProximaFaseOuFim();
    }

    // Recomeça a fase em que o jogador estava
    public void JogarNovamente()
    {
        GameSession.CarregarFaseAtual();
    }

    public void VoltarMenu()
    {
        // As telas de resultado não fazem mais parte do fluxo do jogo.
        GameSession.EncerrarFase();
    }
}
