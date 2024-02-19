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
