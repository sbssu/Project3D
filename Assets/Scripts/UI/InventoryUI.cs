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
        // 스위칭
        // GameObject.activeSelf:bool => 활성화 여부
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
        // 사각형의 각 정점의 월드 위치를 가져온다. (0:좌측하단, 1:좌측상단, 2:우측상단, 3:우측하단)
        Vector3[] corners = new Vector3[4];
        quickSlot.GetWorldCorners(corners);

        // sizeDelta의 경우 로컬 기준 너비를 반환하기 때문에 scaling에 대응할 수 없다.
        // 따라서 world좌표 기준 정점인 corners의 위치 값을 기준으로 월드 너비를 계산한다.

        // float quickWidth = quickSlot.sizeDelta.x;
        float quickWidth = Vector3.Distance(corners[0], corners[3]);

        float slotSize = quickWidth / 9f;               // 슬롯 하나의 사이즈.
        float startX = corners[0].x + slotSize / 2f;    // 첫 시작 x축 위치.
        Vector3 position = quickSelected.position;      // 현재 위치.
        position.x = startX + slotSize * index;         // index를 기반으로 계산한 x축 위치 대입.

        quickSelected.position = position;              // 위치 변경.
    }
}
