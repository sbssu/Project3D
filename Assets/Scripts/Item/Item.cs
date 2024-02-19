using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name => itemData.name;
    public Sprite sprite => itemData.ItemSprite;
    public int count { get; private set; }

    ItemData itemData;

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

    /*
     * public Item(string csv)
    {
        // Trim():문자열의 앞 뒤 공백 제거
        // Split():특정 문자를 기준으로 문자열 자르기.
        string[] datas = csv.Trim().Split(',');
        id = datas[0];
        name = datas[1];

        Sprite[] itemSprites = Resources.LoadAll<Sprite>("Sprites/items");
        sprite = System.Array.Find(itemSprites, spr => spr.name == datas[2]);
        count = 1;
    }
     */
}
