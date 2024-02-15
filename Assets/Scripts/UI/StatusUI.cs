using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusUI : MonoBehaviour
{
    SpriteChange[] changes;
    
    public void ChangeStatus(int value)
    {
        if(changes == null)
            changes = GetComponentsInChildren<SpriteChange>();

        // 고정적으로 총 20의 max를 가진다.
        // change하나당 2의 값을 가진다.
        int fill = value / 2;
        for (int i = 0; i < 10; i++)
        {
            // 0:empty, 1:half, 2:full
            changes[i].ChangeSprite(i < fill ? 2 : 0);
        }

        // 반칸 채우기.
        bool isHalf = value % 2 == 1;
        if (isHalf)
            changes[fill].ChangeSprite(1);
    }
}
