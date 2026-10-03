using UnityEngine;

public class Boss : MonoBehaviour
{
    public int vida = 100;

    public bool estaVivo = true;

    public void ReceberDano(int dano)
    {
        if (!estaVivo)
            return;

        vida -= dano;

        if (vida < 0)
            vida = 0;

        Debug.Log("Vida do Boss: " + vida);

        if (vida <= 0)
        {
            estaVivo = false;
            Debug.Log("Boss derrotado!");
        }
    }
}