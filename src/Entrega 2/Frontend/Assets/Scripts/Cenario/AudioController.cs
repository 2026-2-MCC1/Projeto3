using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [Header("Configurações de Áudio")]
    public AudioSource audioGlobal;

    void Start()
    {
        // Se não arrastaste nenhum AudioSource manualmente, 
        // ele tenta encontrar um automaticamente na cena
        if (audioGlobal == null)
        {
            audioGlobal = FindFirstObjectByType<AudioSource>();
        }
    }

    public void AlterarVolume(float valor)
    {
        if (audioGlobal != null)
        {
            audioGlobal.volume = valor;
        }

        // Guarda a preferência do jogador
        PlayerPrefs.SetFloat("VolumeJogo", valor);
    }
}