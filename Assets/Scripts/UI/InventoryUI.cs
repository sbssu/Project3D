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

    // ���콺 �̺�Ʈ.
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
        // ����Ī
        // GameObject.activeSelf:bool => Ȱ��ȭ ����
        Switch(!panel.activeSelf);
    }
    private void Switch(bool isShow)
    {
        panel.SetActive(isShow);
        Cursor.lockState = panel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        GameValue.isLockRotate = panel.activeSelf;
    }

    // ���� ������Ʈ.
    public void UpdateUI(Item[] items)
    {
        for(int i = 0; i<allSlots.Length; i++)
            allSlots[i].UpdateSlot(items[i]);

        for (int i = 0; i < 9; i++)
            quickBarSlots[i].UpdateSlot(allSlots[i]);
    }
    
    // ������ �ε���.
    public void UpdateQuickIndex(int index)
    {
        // �簢���� �� ������ ���� ��ġ�� �����´�. (0:�����ϴ�, 1:�������, 2:�������, 3:�����ϴ�)
        Vector3[] corners = new Vector3[4];
        quickBar.GetWorldCorners(corners);

        // sizeDelta�� ��� ���� ���� �ʺ� ��ȯ�ϱ� ������ scaling�� ������ �� ����.
        // ���� world��ǥ ���� ������ corners�� ��ġ ���� �������� ���� �ʺ� ����Ѵ�.

        // float quickWidth = quickSlot.sizeDelta.x;
        float quickWidth = Vector3.Distance(corners[0], corners[3]);

        float slotSize = quickWidth / 9f;               // ���� �ϳ��� ������.
        float startX = corners[0].x + slotSize / 2f;    // ù ���� x�� ��ġ.
        Vector3 position = quickBarSelection.position;      // ���� ��ġ.
        position.x = startX + slotSize * index;         // index�� ������� ����� x�� ��ġ ����.

        quickBarSelection.position = position;              // ��ġ ����.
    }


}
