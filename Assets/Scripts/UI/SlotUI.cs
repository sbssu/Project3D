using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// �Ʒ��� �������̽��� Canvas�� GraphicRaycaster�� ȣ���Ѵ�.
public class SlotUI : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] CanvasGroup group;
    [SerializeField] Image image;
    [SerializeField] Text countText;

    Item item;
    int slotIndex;

    public void Setup(int slotIndex)
    {
        this.slotIndex = slotIndex;
        UpdateSlot((Item)null);
    }
    public void UpdateSlot(SlotUI slotUI)
    {
        UpdateSlot(slotUI.item);
    }
    public void UpdateSlot(Item item)
    {
        this.item = item;
        if(item == null)
        {
            image.enabled = false;
            countText.enabled = false;
            return;
        }

        image.enabled = item.sprite != null;
        countText.enabled = item.count > 1;

        image.sprite = item.sprite;
        countText.text = item.count.ToString();
    }

    static int lastSlotIndex = 0;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        // ���콺�� slot�� ������ �� ȣ��Ǵ� �̺�Ʈ
        lastSlotIndex = slotIndex;
    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        // �巡�� ���� �̺�Ʈ. (���� �� : �ε����� ���콺 ��ư ����)
        group.alpha = 0.5f;
        beginDragEvent?.Invoke((int)eventData.button, slotIndex, item);
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        // �巡�װ� ��� �̾����� ȣ��Ǵ� �̺�Ʈ.
        dragEnvet?.Invoke();
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ���� �̺�Ʈ. (���� �� : ���� ���� �ε���)
        group.alpha = 1.0f;
        endDragEvent?.Invoke(lastSlotIndex);
    }
    public delegate void BegenDragHandler(int button, int slotIndex, Item itme);
    public event BegenDragHandler beginDragEvent;
    public event Action dragEnvet;
    public event Action<int> endDragEvent;
}
