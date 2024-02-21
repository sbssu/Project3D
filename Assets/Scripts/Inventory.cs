using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    private enum TYPE
    {
        Inven,
        Quick,
        Equip,
    }

    [SerializeField] ItemDB itemDB;

    // item�� ������ -> �κ� -> ��� ������ ���ĵǾ� �ִ�.
    [System.NonSerialized]
    Item[] items = new Item[9 + 9 * 3 + 4];

    Item currentHandItem;     // ���� �տ� ��� �ִ� ������.
    int handIndex = 0;        // ������ ��ȣ (�� ��ȣ)

    private void Start()
    {
        AddItem("block:dirt");
        AddItem("block:dirt");
        AddItem("block:dirt");
        AddItem("block:sand");
        AddItem("block:sand");
    }
    private void Update()
    {
        if (GameValue.isLockControl)
            return;

        // �� ���� ����.
        float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
        if (wheel != 0)
            UpdateQuickIndex(wheel < 0);

        if (Input.GetKeyDown(KeyCode.E))
            InventoryUI.Instance.Switch();

        // ���� �����ӿ� ����ִ� �����۰� ���� �������� �ٸ� ���.
        if (items[handIndex] != currentHandItem)
        {
            currentHandItem = items[handIndex];
            Player.Instance.UpdateHandItem(currentHandItem);
        }
    }

    public void AddItem(string itemCode)
    {
        Item item = new Item(itemDB.GetItemData(itemCode));
        AddItem(item);
    }
    public bool AddItem(Item newItem)
    {
        bool isSuccess = false;

        // 1.���� �������� ã�� �ִٸ� �����Ѵ�.
        // 2.���� �����ϰ� ������ ������ �ٽ� ã�´�.
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].ID == newItem.ID)
            {
                if (items[i].Combine(newItem))
                {
                    isSuccess = true;
                    break;
                }
            }
        }

        // ���� �߰��Ϸ��� �������� ������ �����ִ�.
        if (newItem.count > 0)
        {
            // 3.��� �迭�� ���鼭 Combine�� �õ��������� ������ ������ �����ִٸ� �� �迭�� �����Ѵ�.
            int emptyIndex = System.Array.FindIndex(items, item => item == null);
            if (emptyIndex < 0)
            {
                Debug.Log($"can't add item '{newItem.name}({newItem.count})' because inventory is full!");
            }
            else
            {
                items[emptyIndex] = newItem;
                isSuccess = true;
            }
        }

        InventoryUI.Instance.UpdateUI(items);
        InventoryUI.Instance.UpdateQuickIndex(handIndex);
        return isSuccess;
    }
    public Item RemoveAtItem(int index, int count = 1)
    {
        if (items[index] == null)
            return null;

        Item item = items[handIndex];       // handIndex�� �ش��ϴ� ������ ����.
        if(item.Substrct(count))            // count������ŭ ������ ����.
            items[handIndex] = null;        // handIndex ���Կ� null ����.

        // UI ������Ʈ.
        InventoryUI.Instance.UpdateUI(items);
        InventoryUI.Instance.UpdateQuickIndex(handIndex);

        return item;
    }

    public BlockObject GetHandItem()
    {
        Item item = RemoveAtItem(handIndex);        // �տ� �ִ� ������ ��������.
        if (item == null)
            return null;

        // �տ� �� �������� ������ ��Ͽ�����Ʈ�� ���� �� ����.
        return ItemManager.Instance.GetBlockObject(item.ID);
    }
    public void UpdateQuickIndex(bool isLeft)
    {
        handIndex += (isLeft ? -1 : 1);
        if (handIndex < 0)
            handIndex = 8;
        else if (handIndex > 8)
            handIndex = 0;

        InventoryUI.Instance.UpdateQuickIndex(handIndex);
    }
    public void DragItem(int start, int end)
    {
        Item temp = items[start];
        items[start] = items[end];
        items[end] = temp;

        InventoryUI.Instance.UpdateUI(items);
    }
        
}
