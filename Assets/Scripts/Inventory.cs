using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    string[] slots = new string[9 * 3];
    string[] equips = new string[4];
    string[] quicks = new string[9];
    int quickIndex = 0;

    public void UpdateQuickIndex(bool isLeft)
    {
        quickIndex += (isLeft ? -1 : 1);
        if (quickIndex < 0)
            quickIndex = 8;
        else if (quickIndex > 8)
            quickIndex = 0;

        InventoryUI.Instance.UpdateQuickIndex(quickIndex);
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
}
