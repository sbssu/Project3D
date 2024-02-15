using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public const int MAX_HP = 20;
    public const int MAX_FOOD = 20;

    [SerializeField] int hp;
    [SerializeField] int food;
    [SerializeField] int level;
    [SerializeField] float exp;

    float[] expTable = { 0, 100, 124, 137, 158, 200, 214, 365, 400 };
    float starveTime = 2.5f;
    float eatTime = 0.0f;

    BottomBarUI bottomUI;

    void Start()
    {
        bottomUI = BottomBarUI.Instance;

        hp = Mathf.Clamp(hp, 1, MAX_HP);
        food = Mathf.Clamp(food, 1, MAX_FOOD);
        level = Mathf.Clamp(level, 1, int.MaxValue);
        exp = Mathf.Clamp(exp, 0, float.MaxValue);

        bottomUI.UpdateHealth(hp);
        bottomUI.UpdateFood(food);
        bottomUI.UpdateLevel(level);
        bottomUI.UpdateExp(exp, expTable[level]);
    }
    private void Update()
    {
        if(Time.time - eatTime >= starveTime)
        {
            eatTime = Time.time;
            food -= 1;
            bottomUI.UpdateFood(food);
        }
    }

}
