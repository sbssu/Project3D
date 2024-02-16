using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public string id;
    public int count;
    public Sprite sprite;

    private Item()
    {

    }
    public Item(string csv)
    {
        // Trim():���ڿ��� �� �� ���� ����
        // Split():Ư�� ���ڸ� �������� ���ڿ� �ڸ���.
        string[] datas = csv.Trim().Split(',');
        id = datas[0];
        name = datas[1];

        Sprite[] itemSprites = Resources.LoadAll<Sprite>("items");
        sprite = System.Array.Find(itemSprites, spr => spr.name == datas[2]);
        count = 1;
    }
    public Item Copy()
    {
        Item item = new Item();
        item.name = name;
        item.id = id;
        item.count = count;
        item.sprite = sprite;
        return item;
    }
}
