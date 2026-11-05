using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIGradient : BaseMeshEffect
{
    [Header("Cores do Degradê")]
    public Color corInicio = Color.cyan;
    public Color corFim = Color.blue;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;

        var rectTransform = GetComponent<RectTransform>();
        float uiHeight = rectTransform.rect.height;

        UIVertex vertex = new UIVertex();
        for (int i = 0; i < vh.currentVertCount; i++)
        {
            vh.PopulateUIVertex(ref vertex, i);

            float yPos = vertex.position.y - rectTransform.rect.yMin;
            float normalizedY = Mathf.Clamp01(yPos / uiHeight); // Corrigido para Clamp01

            vertex.color = Color.Lerp(corFim, corInicio, normalizedY);
            vh.SetUIVertex(vertex, i);
        }
    }
}