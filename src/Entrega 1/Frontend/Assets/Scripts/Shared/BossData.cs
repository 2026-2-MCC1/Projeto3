using UnityEngine;

[CreateAssetMenu(fileName = "NewBoss", menuName = "TurtleHero/Boss Data")]
public class BossData : ScriptableObject
{
    [Header("Identificação")]
    public string nome;

    [Header("Vida e dano")]
    public int vidaMaxima;
    public int danoPerfeito;
    public int danoBom;
}