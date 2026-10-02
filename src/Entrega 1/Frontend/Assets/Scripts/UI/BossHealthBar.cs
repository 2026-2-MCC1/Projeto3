
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (fillImage == null || maxHealth <= 0)
            return;

        fillImage.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }

    private void Start()
    {
        UpdateHealth(50f, 100f);
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            UpdateHealth(100f, 100f);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            UpdateHealth(50f, 100f);

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
            UpdateHealth(25f, 100f);
    }
}
