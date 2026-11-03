using UnityEngine;

// Faz o rio de chocolate "correr" na direção da câmera (rola a textura do material).
public class ChocolateFlow : MonoBehaviour
{
    public Vector2 velocidade = new Vector2(0f, -0.08f);

    Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (material == null) return;
        material.SetTextureOffset("_BaseMap", velocidade * Time.time);
    }
}
