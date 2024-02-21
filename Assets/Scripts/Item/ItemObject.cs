using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IObject<ItemObject>
{
    [SerializeField] string id;

    Transform mainCam;
    SpriteRenderer spriteRenderer;

    new Rigidbody rigidbody;
    new Collider collider;

    Action<ItemObject> IObject<ItemObject>.returnPool { get; set; }

    private void Start()
    {
        mainCam = Camera.main.transform;
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }
    private void Update()
    {
        transform.LookAt(mainCam.position);
    }

    public void Setup(string id)
    {
        ItemData itemData = ItemDB.Instance.GetItemData(id);

        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemData.ItemSprite;
    }
    [ContextMenu("Apply ID")]
    private void Setup()
    {
        Setup(id);
    }

    public void Destroy()
    {
        (this as IObject<ItemObject>).returnPool(this);
    }
    public void EatItem(Player owner, Action<string> addItem)
    {        
        StartCoroutine(IEEat(owner, addItem));
    }
    private IEnumerator IEEat(Player owner, Action<string> addItem)
    {
        rigidbody.isKinematic = true;
        collider.enabled = false;

        while(transform.position != owner.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, owner.transform.position, 10f * Time.deltaTime);
            yield return null;
        }

        ItemData itemData = ItemDB.Instance.GetItemData(id);
        addItem(itemData.ID);
        (this as IObject<ItemObject>).returnPool(this);
    }
}
