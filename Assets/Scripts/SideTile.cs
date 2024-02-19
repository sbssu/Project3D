using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Tile/SideTile")]
public class SideTile : Tile
{
    [SerializeField] Color topColor;
    [SerializeField] Texture top;
    [SerializeField] Texture bottom;

    public override void ToBlock(MeshRenderer[] renderers)
    {
        renderers[0].material.mainTexture = top;
        renderers[0].material.color = topColor;

        // side.
        for (int i = 1; i <= 4; i++)
            renderers[i].material.mainTexture = texture;

        renderers[5].material.mainTexture = bottom;
    }
}
