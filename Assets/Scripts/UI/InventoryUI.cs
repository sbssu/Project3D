using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : Singleton<InventoryUI>
{
    [SerializeField] GameObject panel;

    [Header("quick")]
    [SerializeField] RectTransform quickSlot;
    [SerializeField] RectTransform quickSelected;

    private void Start()
    {
        Switch(false);
    }

    public void Switch(bool isForce = false)
    {
        // ����Ī
        // GameObject.activeSelf:bool => Ȱ��ȭ ����
        if (isForce)
            panel.SetActive(isForce);
        else
            panel.SetActive(!panel.activeSelf);

        Cursor.lockState = panel.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        GameValue.isLockRotate = panel.activeSelf;
    }
    public void UpdateInven(string[] items)
    {

    }
    public void UpdateQuick(string[] items)
    {

    }
    public void UpdateEquip(string[] items)
    {

    }
    public void UpdateQuickIndex(int index)
    {
        // �簢���� �� ������ ���� ��ġ�� �����´�. (0:�����ϴ�, 1:�������, 2:�������, 3:�����ϴ�)
        Vector3[] corners = new Vector3[4];
        quickSlot.GetWorldCorners(corners);

        // sizeDelta�� ��� ���� ���� �ʺ� ��ȯ�ϱ� ������ scaling�� ������ �� ����.
        // ���� world��ǥ ���� ������ corners�� ��ġ ���� �������� ���� �ʺ� ����Ѵ�.

        // float quickWidth = quickSlot.sizeDelta.x;
        float quickWidth = Vector3.Distance(corners[0], corners[3]);

        float slotSize = quickWidth / 9f;               // ���� �ϳ��� ������.
        float startX = corners[0].x + slotSize / 2f;    // ù ���� x�� ��ġ.
        Vector3 position = quickSelected.position;      // ���� ��ġ.
        position.x = startX + slotSize * index;         // index�� ������� ����� x�� ��ġ ����.

        quickSelected.position = position;              // ��ġ ����.
    }
}
