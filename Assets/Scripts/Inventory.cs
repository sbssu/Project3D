using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    [SerializeField] ItemDB itemDB;
        

    [System.NonSerialized] Item[] slots = new Item[9 * 3];
    [System.NonSerialized] Item[] equips = new Item[4];
    [System.NonSerialized] Item[] quicks = new Item[9];
    int quickIndex = 0;


    private void Start()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i] = null;
        for (int i = 0; i < equips.Length; i++)
            equips[i] = null;
        for (int i = 0; i < slots.Length; i++)
            slots[i] = null;

        AddItem("block:dirt");
    }
    private void Update()
    {
        if (GameValue.isLockControl)
            return;

        // Äü ½½·Ô Á¶ÀÛ.
        float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
        if (wheel != 0)
            UpdateQuickIndex(wheel < 0);

        if (Input.GetKeyDown(KeyCode.E))
            InventoryUI.Instance.Switch();
    }

    public void AddItem(string itemCode)
    {
        Item item = new Item(itemDB.GetItemData(itemCode));
        AddItem(item);
    }
    public void AddItem(Item item)
    {
        int index = System.Array.FindIndex(slots, slot => slot == null);
        if (index >= 0)
            slots[index] = item;

        InventoryUI.Instance.UpdateInven(slots);
    }
    public void UpdateQuickIndex(bool isLeft)
    {
        quickIndex += (isLeft ? -1 : 1);
        if (quickIndex < 0)
            quickIndex = 8;
        else if (quickIndex > 8)
            quickIndex = 0;

        InventoryUI.Instance.UpdateQuickIndex(quickIndex);
    }
}
