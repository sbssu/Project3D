using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryUI : Singleton<InventoryUI>
{
    [SerializeField] GameObject panel;
    [SerializeField] RectTransform invenParent;
    [SerializeField] RectTransform quickParent;
    [SerializeField] RectTransform equipParnet;
    [SerializeField] RectTransform quickBarParent;
    [SerializeField] SlotUI previewSlot;

    [Header("quick")]
    [SerializeField] RectTransform quickBar;
    [SerializeField] RectTransform quickBarSelection;

    SlotUI[] allSlots;
    SlotUI[] quickBarSlots;

    protected new void Awake()
    {
        base.Awake();

        SlotUI[] quickSlots = quickParent.GetComponentsInChildren<SlotUI>();
        SlotUI[] invenSlots = invenParent.GetComponentsInChildren<SlotUI>();
        SlotUI[] equipSlots = equipParnet.GetComponentsInChildren<SlotUI>();

        List<SlotUI> slotList = new List<SlotUI>();
        slotList.AddRange(quickSlots);
        slotList.AddRange(invenSlots);
        slotList.AddRange(equipSlots);
        allSlots = slotList.ToArray();

        int index = 0;
        foreach (SlotUI slot in allSlots)
        {
            // regested event.
            slot.beginDragEvent += BeginDragSlot;
            slot.dragEnvet += DragSlot;
            slot.endDragEvent += EndDragSlot;

            // initialize.
            slot.Setup(index++);
        }

        quickBarSlots = quickBarParent.GetComponentsInChildren<SlotUI>();
    }
    private void Start()
    {
        previewSlot.gameObject.SetActive(false);
        Switch(false);
    }

    // 마우스 이벤트.
    private int startDragIndex;
    private void BeginDragSlot(int button, int slotIndex, Item item)
    {
        startDragIndex = slotIndex;
        previewSlot.UpdateSlot(item);
        previewSlot.gameObject.SetActive(true);
    }
    private void DragSlot()
    {
        previewSlot.transform.position = Input.mousePosition;
    }
    private void EndDragSlot(int slotIndex)
    {
        previewSlot.gameObject.SetActive(false);
        Inventory.Instance.DragItem(startDragIndex, slotIndex);
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
    public void UpdateUI(Item[] items)
    {
        for(int i = 0; i<allSlots.Length; i++)
            allSlots[i].UpdateSlot(items[i]);

        for (int i = 0; i < 9; i++)
            quickBarSlots[i].UpdateSlot(allSlots[i]);
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
