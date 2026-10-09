using UnityEngine;
using UnityEngine.InputSystem;

// 4 pistas = 4 teclas. Só dispara o evento; quem decide acerto/erro é o JudgmentSystem
public class InputManager : MonoBehaviour
{
    public Key[] laneKeys = { Key.D, Key.F, Key.J, Key.K };

    void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        for (int i = 0; i < laneKeys.Length; i++)
        {
            if (kb[laneKeys[i]].wasPressedThisFrame)
                GameEvents.LanePressed(i);
        }
    }
}
