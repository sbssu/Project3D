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

    // item은 퀵슬롯 -> 인벤 -> 장비 순으로 정렬되어 있다.
    [System.NonSerialized]
    Item[] items = new Item[9 + 9 * 3 + 4];

    Item currentHandItem;     // 현재 손에 들고 있는 아이템.
    int handIndex = 0;        // 퀵슬롯 번호 (손 번호)

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

        // 퀵 슬롯 조작.
        float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
        if (wheel != 0)
            UpdateQuickIndex(wheel < 0);

        if (Input.GetKeyDown(KeyCode.E))
            InventoryUI.Instance.Switch();

        // 이전 프레임에 들고있던 아이템과 현재 아이템이 다를 경우.
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

        // 1.같은 아이템을 찾고 있다면 병합한다.
        // 2.만약 병합하고도 개수가 남으면 다시 찾는다.
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

        // 아직 추가하려는 아이템의 개수가 남아있다.
        if (newItem.count > 0)
        {
            // 3.모든 배열을 돌면서 Combine을 시도했음에도 여전히 개수가 남아있다면 빈 배열에 대입한다.
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

        Item item = items[handIndex];       // handIndex에 해당하는 아이템 참조.
        if(item.Substrct(count))            // count개수만큼 아이템 빼기.
            items[handIndex] = null;        // handIndex 슬롯에 null 대입.

        // UI 업데이트.
        InventoryUI.Instance.UpdateUI(items);
        InventoryUI.Instance.UpdateQuickIndex(handIndex);

        return item;
    }

    public BlockObject GetHandItem()
    {
        Item item = RemoveAtItem(handIndex);        // 손에 있는 아이템 꺼내오기.
        if (item == null)
            return null;

        // 손에 쥔 아이템을 꺼내서 블록오브젝트로 변경 후 전달.
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
