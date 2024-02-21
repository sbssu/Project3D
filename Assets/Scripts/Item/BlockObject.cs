using System;
using UnityEngine;

public class BlockObject : MonoBehaviour, IObject<BlockObject>
{
    [SerializeField] MeshRenderer[] planes;
    [SerializeField] string id;

    public string ID => id;
    Action<BlockObject> IObject<BlockObject>.returnPool { get; set; }

    public void Setup(string id)
    {
        this.id = id;
        ItemData itemData = ItemDB.Instance.GetItemData(id);
        itemData.ApplyTile(planes);
    }
    public void Destroy()
    {
        Action<BlockObject> callback = ((IObject<BlockObject>)this).returnPool;
        if (callback == null)
            Destroy(gameObject);
        else
            callback(this);
    }

    [ContextMenu("Apply ID")]
    private void Setup()
    {
        Setup(id);
    }
}
