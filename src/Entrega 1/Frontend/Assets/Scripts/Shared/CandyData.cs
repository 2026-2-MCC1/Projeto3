using UnityEngine;

[CreateAssetMenu(fileName = "NewCandy", menuName = "TurtleHero/Candy Data")]
public class CandyData : ScriptableObject
{
    [Header("Identificação")]
    public string nome;
    public int fase;

    [Header("Conteúdo")]
    [TextArea]
    public string curiosidade;
}