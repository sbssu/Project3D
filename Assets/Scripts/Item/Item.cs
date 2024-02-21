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

    // 같은 아이템을 합치기 위한 함수 : 반환값은 완벽하게 합쳐졌는가?
    public bool Combine(Item item)
    {
        if (count + item.count <= MAX_COUNT)
        {
            // 병합 후 개수가 최대보다 적을 때.
            count += item.count;
            item.count = 0;
            return true;
        }
        else
        {
            // 병합 후 개수가 최대보다 많을 때.
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
