using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorMenu : MonoBehaviour
{
    // --- FLUXO INICIAL ---
    public void IrParaHistoria()
    {
        SceneManager.LoadScene("Historia");
    }

    // --- FASE 1 ---
    public void IrParaRecompensas1() { SceneManager.LoadScene("Recompensas1"); }
    public void ColetarESeguirFase2() { SceneManager.LoadScene("Phase2"); }
    public void TenteNovamentePhase1() { SceneManager.LoadScene("Phase1"); }

    // --- FASE 2 ---
    public void IrParaRecompensas2() { SceneManager.LoadScene("Recompensas2"); }
    public void ColetarESeguirFase3() { SceneManager.LoadScene("Phase3"); }
    public void TenteNovamentePhase2() { SceneManager.LoadScene("Phase2"); }

    // --- FASE 3 ---
    public void IrParaRecompensas3() { SceneManager.LoadScene("Recompensas3"); }
    public void ColetarESeguirFinal() { SceneManager.LoadScene("Final"); }
    public void TenteNovamentePhase3() { SceneManager.LoadScene("Phase3"); }

    // --- TELA FINAL E CRÉDITOS ---
    public void IrParaCreditos() { SceneManager.LoadScene("Creditos"); }

    // --- BOTÃO UNIVERSAL DE MENU ---
    public void VoltarParaMenu()
    {
        SceneManager.LoadScene("MenuInicial");
    }
}