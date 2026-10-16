using UnityEngine;

public class HitZone : MonoBehaviour
{
    void OnEnable() { 
        GameEvents.OnNoteJudged += AoJulgar; 
    }
    void OnDisable() { 
        GameEvents.OnNoteJudged -= AoJulgar; 
    }

    void AoJulgar(Judgment resultado)
    {
        if (resultado == Judgment.Miss) Debug.Log("ERROU!");
        else Debug.Log("ACERTOU!");
    }
}