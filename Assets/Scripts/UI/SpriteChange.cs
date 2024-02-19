using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChange : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Sprite[] sprites;

    /// <summary>
    /// ��������Ʈ�� �����Ѵ�.
    /// </summary>
    /// <param name="index"></param>
    public void ChangeSprite(int index)
    {
        image.sprite = sprites[index];
    }
}
