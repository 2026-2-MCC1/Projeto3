using UnityEngine;

public class BossController : MonoBehaviour
{
    public BossData bossData;

    private int currentHealth;

    void Start()
    {
        currentHealth = bossData.maxHealth;

        Debug.Log("Boss started with " + currentHealth + " health.");
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Boss took " + damage + " damage.");
        Debug.Log("Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("BOSS DEFEATED!");

        gameObject.SetActive(false);
    }
}