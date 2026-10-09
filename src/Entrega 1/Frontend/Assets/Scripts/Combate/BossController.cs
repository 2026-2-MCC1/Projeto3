using UnityEngine;
using UnityEngine.InputSystem;

public class BossController : MonoBehaviour
{
    public BossData bossData;
    public bool debugKeys = false;   // A = dano perfeito, S = dano bom (só para teste isolado)

    int currentHealth;
    bool defeated;

    void OnEnable() { GameEvents.OnNoteJudged += HandleJudgment; }
    void OnDisable() { GameEvents.OnNoteJudged -= HandleJudgment; }

    void Start()
    {
        currentHealth = bossData.maxHealth;
        GameEvents.BossHealthChanged(currentHealth, bossData.maxHealth);
    }

    void HandleJudgment(Judgment j)
    {
        switch (j)
        {
            case Judgment.Perfect: TakeDamage(bossData.perfectDamage); break;
            case Judgment.Good: TakeDamage(bossData.goodDamage); break;
            case Judgment.Miss: break; // Errou = 0 de dano (a vida do jogador é no PlayerLives)
        }
    }

    void Update()
    {
        if (!debugKeys || Keyboard.current == null) return;
        if (Keyboard.current.aKey.wasPressedThisFrame) TakeDamage(bossData.perfectDamage);
        if (Keyboard.current.sKey.wasPressedThisFrame) TakeDamage(bossData.goodDamage);
    }

    public void TakeDamage(int damage)
    {
        if (defeated || GameStateManager.Current != GameState.Playing) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        GameEvents.BossHealthChanged(currentHealth, bossData.maxHealth);

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        defeated = true;
        GameEvents.BossDefeated(PhaseManager.CurrentPhase);
        gameObject.SetActive(false);
    }
}
