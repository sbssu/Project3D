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
    BlockObject handItemObject;      // �տ� ����ִ� ������.

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
        // ����� ������.
        if(Time.time - eatTime >= starveTime)
        {
            eatTime = Time.time;
            food -= 1;
            statusUI.UpdateFood(food);
        }

        // ī�޶� ���� ��ȣ�ۿ� ������Ʈ �˻�.
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, rayDistance, blockMask))
        {
            // ���콺 ���� Ŭ��.
            if(Input.GetMouseButtonDown(0))
            {
                BlockObject block = hit.collider.GetComponent<BlockObject>();       // �浹�� ��ü�� ������Ʈ �˻�.
                string id = block.ID;                                               // ID �˻�.
                block.Destroy();                                                    // ��� ����.
                Inventory.Instance.AddItem(id);                                     // ������ �߰�.
            }

            // ���콺 ���� Ŭ��.
            else if(Input.GetMouseButtonDown(1))
            {
                BlockObject handBlock = Inventory.Instance.GetHandItem();
                if(handBlock != null)
                {
                    // ��ġ�� ����� ��ġ,ȸ��,������ �� ����.
                    Transform hitTransform = hit.collider.transform;
                    handBlock.transform.position = hitTransform.position + hit.normal;
                    handBlock.transform.rotation = hitTransform.rotation;
                    handBlock.transform.localScale = hitTransform.localScale;
                }
            }
        }

        // ������ ������Ʈ ����.
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

        // ���� �տ� ��� ���µ� ���ο� �������� �����ϴ� ���.
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
