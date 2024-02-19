using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Tile/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] protected string id;
    [SerializeField] protected string itemName;
    [SerializeField] protected Sprite itemSprite;
    [SerializeField] protected Texture texture;

    public string ID => id;
    public string ItemName => itemName;
    public Sprite ItemSprite => itemSprite;

    public virtual void ApplyTile(MeshRenderer[] renderers)
    {
        for (int i = 0; i < 6; i++)
            renderers[i].sharedMaterial.mainTexture = texture;

        renderers[0].sharedMaterial.color = Color.white;
    }
}
