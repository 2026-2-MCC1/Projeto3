using UnityEngine;
using UnityEngine.InputSystem;

public class Button : MonoBehaviour
{
    public enum Tecla
    {
        A,
        S,
        K,
        L
    }

    public Tecla tecla;
    public Boss boss;

    void Update()
    {
        bool pressionou = false;

        switch (tecla)
        {
            case Tecla.A:
                pressionou = Keyboard.current.aKey.wasPressedThisFrame;
                break;

            case Tecla.S:
                pressionou = Keyboard.current.sKey.wasPressedThisFrame;
                break;

            case Tecla.K:
                pressionou = Keyboard.current.kKey.wasPressedThisFrame;
                break;

            case Tecla.L:
                pressionou = Keyboard.current.lKey.wasPressedThisFrame;
                break;
        }

        if (pressionou)
        {
            boss.ReceberDano(10);
            Debug.Log("Botão pressionado: " + tecla);
        }
    }
}