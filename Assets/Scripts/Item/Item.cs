using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public static readonly int MAX_COUNT = 64;
    private ItemData itemData;

    public string ID => itemData.ID;
    public string name => itemData.name;
    public Sprite sprite => itemData.ItemSprite;
    public int count { get; private set; }

    public Item(ItemData itemData)
    {
        this.itemData = itemData;
        count = 1;
    }

    // ���� �������� ��ġ�� ���� �Լ� : ��ȯ���� �Ϻ��ϰ� �������°�?
    public bool Combine(Item item)
    {
        if (count + item.count <= MAX_COUNT)
        {
            // ���� �� ������ �ִ뺸�� ���� ��.
            count += item.count;
            item.count = 0;
            return true;
        }
        else
        {
            // ���� �� ������ �ִ뺸�� ���� ��.
            int over = (count + item.count) - MAX_COUNT;
            count = MAX_COUNT;
            item.count -= over;
            return false;
        }
    }
    public bool Substrct(int amount)
    {
        if(count - amount <= 0)
        {
            count = 0;
            return true;
        }
        else
        {
            count -= amount;
            return false;
        }
    }
}
