using UnityEngine;

public class Boss : MonoBehaviour
{
    public int vida = 100;

    public void ReceberDano(int dano)
    {
        vida -= dano;

        Debug.Log("Vida do Boss: " + vida);

        if (vida <= 0)
        {
            Debug.Log("Boss derrotado!");
        }
    }
}