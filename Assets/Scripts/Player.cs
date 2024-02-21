using UnityEngine;


public class Player : Singleton<Player>
{
    public const int MAX_HP = 20;
    public const int MAX_FOOD = 20;

    [SerializeField] int hp;
    [SerializeField] int food;
    [SerializeField] int level;
    [SerializeField] float exp;

    [Header("Position")]
    [SerializeField] Transform handPivot;
    [SerializeField] LayerMask blockMask;

    float[] expTable = { 0, 100, 124, 137, 158, 200, 214, 365, 400 };
    float starveTime = 2.5f;
    float eatTime = 0.0f;

    const float rayDistance = 5;

    Camera cam;
    StatusUI statusUI;
    LayerMask itemObjectMask;
    BlockObject handItemObject;      // 손에 들고있는 아이템.

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
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, rayDistance, blockMask))
        {
            // 마우스 왼쪽 클릭.
            if(Input.GetMouseButtonDown(0))
            {
                BlockObject block = hit.collider.GetComponent<BlockObject>();       // 충돌한 물체의 컴포넌트 검색.
                string id = block.ID;                                               // ID 검색.
                block.Destroy();                                                    // 블록 삭제.
                Inventory.Instance.AddItem(id);                                     // 아이템 추가.
            }

            // 마우스 우측 클릭.
            else if(Input.GetMouseButtonDown(1))
            {
                BlockObject handBlock = Inventory.Instance.GetHandItem();
                if(handBlock != null)
                {
                    // 설치한 블록의 위치,회전,스케일 값 적용.
                    Transform hitTransform = hit.collider.transform;
                    handBlock.transform.position = hitTransform.position + hit.normal;
                    handBlock.transform.rotation = hitTransform.rotation;
                    handBlock.transform.localScale = hitTransform.localScale;
                }
            }
        }

        // 아이템 오브젝트 습득.
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f, itemObjectMask);
        foreach (Collider collider in colliders)
        {
            ItemObject itemObject = collider.GetComponent<ItemObject>();
            itemObject.EatItem(this, Inventory.Instance.AddItem);
        }
    }


    public void UpdateHandItem(Item item)
    {
        if(item == null)
        {
            handItemObject?.Destroy();
            handItemObject = null;
            return;
        }

        // 내가 손에 든게 없는데 새로운 아이템을 들어야하는 경우.
        if (handItemObject == null)
            handItemObject = ItemManager.Instance.GetBlockObject(item.ID);
        else
            handItemObject.Setup(item.ID);

        handItemObject.transform.SetParent(handPivot);
        handItemObject.transform.localPosition = Vector3.zero;
        handItemObject.transform.localRotation = Quaternion.identity;
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
