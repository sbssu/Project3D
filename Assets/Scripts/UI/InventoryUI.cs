using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class InventoryUI : Singleton<InventoryUI>
{
    [SerializeField] GameObject panel;
    [SerializeField] RectTransform invenParent;
    [SerializeField] RectTransform quickParent;
    [SerializeField] RectTransform equipParnet;

    [Header("quick")]
    [SerializeField] RectTransform quickBar;
    [SerializeField] RectTransform quickBarSelection;

    SlotUI[] invenSlots;
    SlotUI[] quickSlots;
    SlotUI[] equipSlots;

    protected new void Awake()
    {
        base.Awake();

        invenSlots = invenParent.GetComponentsInChildren<SlotUI>();
        quickSlots = quickParent.GetComponentsInChildren<SlotUI>();
        equipSlots = equipParnet.GetComponentsInChildren<SlotUI>();

        List<SlotUI> allSlots = new List<SlotUI>();
        allSlots.AddRange(invenSlots);
        allSlots.AddRange(quickSlots);
        allSlots.AddRange(equipSlots);

        foreach (SlotUI slot in allSlots)
            slot.UpdateSlot(null, string.Empty);
    }

    private void Start()
    {
        Switch(false);
    }

    public void Switch()
    {
        // 스위칭
        // GameObject.activeSelf:bool => 활성화 여부
        Switch(!panel.activeSelf);
    }
    private void Switch(bool isShow)
    {
        panel.SetActive(isShow);
        Cursor.lockState = panel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        GameValue.isLockRotate = panel.activeSelf;
    }

    // 슬롯 업데이트.
    public void UpdateInven(Item[] items)
    {
        UpdateSlot(invenSlots, items);
    }
    public void UpdateQuick(Item[] items)
    {
        UpdateSlot(quickSlots, items);
    }
    public void UpdateEquip(Item[] items)
    {
        UpdateSlot(equipSlots, items);
    }
    private void UpdateSlot(SlotUI[] slots, Item[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
                slots[i].UpdateSlot(null, string.Empty);
            else
            {
                Sprite sprite = items[i].sprite;
                string text = (items[i].count < 2) ? string.Empty : items[i].count.ToString();
                slots[i].UpdateSlot(sprite, text);
            }
        }
    }
    
    // 퀵슬롯 인덱스.
    public void UpdateQuickIndex(int index)
    {
        // 사각형의 각 정점의 월드 위치를 가져온다. (0:좌측하단, 1:좌측상단, 2:우측상단, 3:우측하단)
        Vector3[] corners = new Vector3[4];
        quickBar.GetWorldCorners(corners);

        // sizeDelta의 경우 로컬 기준 너비를 반환하기 때문에 scaling에 대응할 수 없다.
        // 따라서 world좌표 기준 정점인 corners의 위치 값을 기준으로 월드 너비를 계산한다.

        // float quickWidth = quickSlot.sizeDelta.x;
        float quickWidth = Vector3.Distance(corners[0], corners[3]);

        float slotSize = quickWidth / 9f;               // 슬롯 하나의 사이즈.
        float startX = corners[0].x + slotSize / 2f;    // 첫 시작 x축 위치.
        Vector3 position = quickBarSelection.position;      // 현재 위치.
        position.x = startX + slotSize * index;         // index를 기반으로 계산한 x축 위치 대입.

        quickBarSelection.position = position;              // 위치 변경.
    }
}
