using UnityEngine;
using UnityEngine.InputSystem;

public class TeclaPista : MonoBehaviour
{
    // Passo 1: escolher qual tecla esse quadrado representa e as duas cores
    // Passo 2: guardar o Renderer (o que desenha o cubo) para poder mudar a cor
    // Passo 3: a cada frame, se a tecla está apertada usa a cor de apertado, senão a cor normal

    public Key tecla;
    public Color corNormal = Color.gray;
    public Color corApertada = Color.yellow;

    Renderer desenho;

    void Start()
    {
        desenho = GetComponent<Renderer>();
    }

    void Update()
    {
        if (Keyboard.current[tecla].isPressed)
        {
            desenho.material.color = corApertada;
        }
        else
        {
            desenho.material.color = corNormal;
        }
    }
}