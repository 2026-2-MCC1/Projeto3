using UnityEngine;

[CreateAssetMenu(fileName = "NewPhaseChart", menuName = "TurtleHero/Phase Chart")]
public class PhaseChartData : ScriptableObject
{
    [Header("Identificação da fase")]
    public int fase;

    [Header("Parâmetros do chart de notas")]
    public float noteSpeed;
    public float noteDensity;
}