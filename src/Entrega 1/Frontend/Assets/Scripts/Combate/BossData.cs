using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "TurtleHero/Boss Data")]
public class BossData : ScriptableObject
{
    public string bossName;

    public int maxHealth = 100;

    public int perfectDamage = 20;
    public int goodDamage = 10;

    public int difficultyWeight = 1;
}