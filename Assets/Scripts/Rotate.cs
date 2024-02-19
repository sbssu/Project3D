using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class MinMax<T> where T : struct
{
    public T min;
    public T max;
    public MinMax(T min, T max)
    {
        this.min = min;
        this.max = max;
    }
}

public class Rotate : MonoBehaviour
{
    [SerializeField] Transform camTransform;
    [SerializeField] Vector2 sensitivity;           // 민감도.
    [SerializeField] MinMax<float> limitRotateX;    // x축 회전 제한값.

    float rotateX;  // x축 회전 값.

    private void Start()
    {
        // 오일러각 : 사람이 이해하는 각 축에 대한 회전
        // 쿼터니언 : 차원 벡터 공간에서 회전을 표현하는 수학적 개념.
        // Quaternion.Euler(Vector3) = 오일러 각을 쿼터니언으로 반환.
        // transform.rotation *= Quaternion.Euler(0, 90, 0);

        // Quaternion.AngleAxis(float, Vector) : 특정 축에 대한 회전량을 구한다.
        // transform.rotation *= Quaternion.AngleAxis(90f, Vector3.up);
    }

    private void Update()
    {
        if (GameValue.isLockControl || GameValue.isLockRotate)
            return;

        float angle = 100f;
        float rotationX = angle * Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;
        float rotationY = angle * Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;

        // 수평에 대한 회전.
        transform.rotation *= Quaternion.AngleAxis(rotationX, Vector3.up);

        // x축에 대한 (수직)회전은 +가 아래쪽으로 돌기 때문에 반대로 뒤집어야한다.
        // rotateX를 통해 최소,최대 사이 값을 계산한다.
        rotateX = Mathf.Clamp(rotateX + rotationY * -1f, limitRotateX.min, limitRotateX.max);
        camTransform.localRotation = Quaternion.Euler(rotateX, 0f, 0f);
    }

}
