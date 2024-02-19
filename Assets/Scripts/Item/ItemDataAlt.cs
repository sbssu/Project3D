using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Tile/ItemDataAlt")]
public class ItemDataAlt : ItemData
{
    [SerializeField] Color topColor;
    [SerializeField] Texture top;
    [SerializeField] Texture bottom;

    public override void ApplyTile(MeshRenderer[] renderers)
    {
        renderers[0].sharedMaterial.mainTexture = top;
        renderers[0].sharedMaterial.color = topColor;

        // side.
        for (int i = 1; i <= 4; i++)
            renderers[i].sharedMaterial.mainTexture = texture;

        renderers[5].sharedMaterial.mainTexture = bottom;
    }
}
