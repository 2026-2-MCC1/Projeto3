
using UnityEngine;
using UnityEngine.InputSystem;

public class DefeatTest : MonoBehaviour
{
    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            GameEvents.RaisePlayerLifeLost();
        }
    }
}
