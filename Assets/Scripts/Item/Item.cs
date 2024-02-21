using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    ItemData itemData;

    public string ID => itemData.ID;
    public string name => itemData.name;
    public Sprite sprite => itemData.ItemSprite;
    public int count { get; private set; }

    private Item()
    {

    }
    public Item(ItemData itemData)
    {
        this.itemData = itemData;
        count = 1;
    }
    public Item Copy()
    {
        Item item = new Item(itemData);
        item.count = count;
        return item;
    }
}
