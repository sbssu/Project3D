using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomBarUI : Singleton<BottomBarUI>
{
    [SerializeField] StatusUI healthUI;
    [SerializeField] StatusUI foodUI;
    [SerializeField] Image expProgress;
    [SerializeField] Text levelText;

    public void UpdateHealth(int value)
    {
        healthUI.ChangeStatus(value);
    }
    public void UpdateFood(int value)
    {
        foodUI.ChangeStatus(value);
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
