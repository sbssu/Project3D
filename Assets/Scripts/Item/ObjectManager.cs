using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    ObjectPoolling<BlockObject> blockStorage;
    ObjectPoolling<ItemObject> itemStorage;

    private new void Awake()
    {
        base.Awake();

        blockStorage = new GameObject("Block Storage").AddComponent<ObjectPoolling<BlockObject>>();
        blockStorage.Initialized("BlockObject", 20);
        blockStorage.transform.SetParent(transform);

        itemStorage = new GameObject("Item Storage").AddComponent<ObjectPoolling<ItemObject>>();
        itemStorage.Initialized("ItemObject", 20);
        itemStorage.transform.SetParent(transform);
    }

    public BlockObject GetBlockObject(string id)
    {
        BlockObject block = blockStorage.GetObject();
        block.Setup(id);
        return block;
    }

}
