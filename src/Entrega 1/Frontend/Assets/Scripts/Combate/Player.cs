using UnityEngine;

public class Player : MonoBehaviour
{
    public Boss boss;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            boss.ReceberDano(10);
            Debug.Log("Botão 1 - A");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            boss.ReceberDano(10);
            Debug.Log("Botão 2 - S");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            boss.ReceberDano(10);
            Debug.Log("Botão 3 - K");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            boss.ReceberDano(10);
            Debug.Log("Botão 4 - L");
        }
    }
}