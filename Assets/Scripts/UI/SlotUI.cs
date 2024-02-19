using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Text countText;

    public void UpdateSlot(Sprite sprite, string text)
    {
        image.enabled = sprite != null;
        countText.enabled = !string.IsNullOrEmpty(text);

        image.sprite = sprite;
        countText.text = text;
    }
}
