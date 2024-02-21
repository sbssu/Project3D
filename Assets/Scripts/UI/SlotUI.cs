using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 아래의 인터페이스는 Canvas의 GraphicRaycaster가 호출한다.
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
        // 마우스가 slot에 들어왔을 때 호출되는 이벤트
        lastSlotIndex = slotIndex;
    }
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        // 드래그 시작 이벤트. (전달 값 : 인덱스와 마우스 버튼 종류)
        group.alpha = 0.5f;
        beginDragEvent?.Invoke((int)eventData.button, slotIndex, item);
    }
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        // 드래그가 계속 이어질때 호출되는 이벤트.
        dragEnvet?.Invoke();
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        // 드래그 종료 이벤트. (전달 값 : 끝난 슬롯 인덱스)
        group.alpha = 1.0f;
        endDragEvent?.Invoke(lastSlotIndex);
    }
    public delegate void BegenDragHandler(int button, int slotIndex, Item itme);
    public event BegenDragHandler beginDragEvent;
    public event Action dragEnvet;
    public event Action<int> endDragEvent;
}
