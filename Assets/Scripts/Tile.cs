using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Tile/Tile")]
public class Tile : ScriptableObject
{
    [SerializeField] protected string id;
    [SerializeField] protected Texture texture;

    public string ID => id;

    public virtual void ToBlock(MeshRenderer[] renderers)
    {
        for(int i = 0; i<6; i++)
            renderers[i].material.mainTexture = texture;
    }
}
