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

    const float rayDistance = 5;

    Camera cam;
    StatusUI statusUI;
    LayerMask itemObjectMask;

    void Start()
    {
        itemObjectMask = 1 << LayerMask.NameToLayer("ItemObject");
        statusUI = StatusUI.Instance;
        cam = Camera.main;

        hp = Mathf.Clamp(hp, 1, MAX_HP);
        food = Mathf.Clamp(food, 1, MAX_FOOD);
        level = Mathf.Clamp(level, 1, int.MaxValue);
        exp = Mathf.Clamp(exp, 0, float.MaxValue);

        statusUI.UpdateHealth(hp);
        statusUI.UpdateFood(food);
        statusUI.UpdateLevel(level);
        statusUI.UpdateExp(exp, expTable[level]);                
    }
    private void Update()
    {
        // 배고픔 게이지.
        if(Time.time - eatTime >= starveTime)
        {
            eatTime = Time.time;
            food -= 1;
            statusUI.UpdateFood(food);
        }

        // 카메라 정면 상호작용 오브젝트 검색.
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, rayDistance))
        {
            IInterct target = hit.collider.GetComponent<IInterct>();
            if (target != null)
            {
                InterectUI.Instance.Setup("F", target.interctName);
                InterectUI.Instance.SwitchUI(true);

                if (Input.GetKeyDown(KeyCode.F))
                    target.Interect(this);
            }
        }
        else
            InterectUI.Instance.SwitchUI(false);

        // 아이템 오브젝트 습득.
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, itemObjectMask);
        foreach (Collider collider in colliders)
        {
            ItemObject itemObject = collider.GetComponent<ItemObject>();
            itemObject.EatItem(this, Inventory.Instance.AddItem);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * rayDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
}

public interface IInterct
{
    string interctName { get; }
    void Interect(object owner);
}
