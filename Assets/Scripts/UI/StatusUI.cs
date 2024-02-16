using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : Singleton<StatusUI>
{
    [SerializeField] RectTransform hpParent;
    [SerializeField] RectTransform foodParent;
    [SerializeField] Image expProgress;
    [SerializeField] Text levelText;

    SpriteChange[] hpSprites;
    SpriteChange[] foodSprites;

    public void UpdateHealth(int value)
    {
        if (hpSprites == null)
            hpSprites = hpParent.GetComponentsInChildren<SpriteChange>();
        ChangeValue(hpSprites, value);
    }
    public void UpdateFood(int value)
    {
        if(foodSprites == null)
            foodSprites = foodParent.GetComponentsInChildren<SpriteChange>();
        ChangeValue(foodSprites, value);
    }
    private void ChangeValue(SpriteChange[] changes, int value)
    {
        // 고정적으로 총 20의 max를 가진다.
        // change하나당 2의 값을 가진다.
        int fill = value / 2;
        for (int i = 0; i < 10; i++)
        {
            // 0:empty, 1:half, 2:full
            changes[i].ChangeSprite(i < fill ? 2 : 0);
        }

        // 반칸 채우기.
        bool isHalf = value % 2 == 1;
        if (isHalf)
            changes[fill].ChangeSprite(1);
    }

    public void UpdateLevel(int value)
    {
        levelText.text = value.ToString();
    }
    public void UpdateExp(float current, float max)
    {
        expProgress.fillAmount = current / max;
    }
    
}
