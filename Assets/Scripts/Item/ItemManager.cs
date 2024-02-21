using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    ObjectPool<BlockObject> blockStorage;
    ObjectPool<ItemObject> itemStorage;

    private new void Awake()
    {
        base.Awake();

        blockStorage = new GameObject("Block Storage").AddComponent<BlockPool>();
        blockStorage.Initialized("BlockObject", 20);
        blockStorage.transform.SetParent(transform);

        itemStorage = new GameObject("Item Storage").AddComponent<ItemPool>();
        itemStorage.Initialized("ItemObject", 20);
        itemStorage.transform.SetParent(transform);
    }

    public BlockObject GetBlockObject(string id)
    {
        BlockObject block = blockStorage.GetObject();
        block.Setup(id);
        return block;
    }
    public ItemObject GetItemObject(string id)
    {
        ItemObject item = itemStorage.GetObject();
        item.Setup(id);
        return item;
    }

}
