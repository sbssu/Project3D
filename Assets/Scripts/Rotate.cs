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
    [SerializeField] Vector2 sensitivity;           // �ΰ���.
    [SerializeField] MinMax<float> limitRotateX;    // x�� ȸ�� ���Ѱ�.

    float rotateX;  // x�� ȸ�� ��.

    private void Start()
    {
        // ���Ϸ��� : ����� �����ϴ� �� �࿡ ���� ȸ��
        // ���ʹϾ� : ���� ���� �������� ȸ���� ǥ���ϴ� ������ ����.
        // Quaternion.Euler(Vector3) = ���Ϸ� ���� ���ʹϾ����� ��ȯ.
        // transform.rotation *= Quaternion.Euler(0, 90, 0);

        // Quaternion.AngleAxis(float, Vector) : Ư�� �࿡ ���� ȸ������ ���Ѵ�.
        // transform.rotation *= Quaternion.AngleAxis(90f, Vector3.up);
    }

    private void Update()
    {
        if (GameValue.isLockControl || GameValue.isLockRotate)
            return;

        float angle = 100f;
        float rotationX = angle * Input.GetAxis("Mouse X") * sensitivity.x * Time.deltaTime;
        float rotationY = angle * Input.GetAxis("Mouse Y") * sensitivity.y * Time.deltaTime;

        // ���� ���� ȸ��.
        transform.rotation *= Quaternion.AngleAxis(rotationX, Vector3.up);

        // x�࿡ ���� (����)ȸ���� +�� �Ʒ������� ���� ������ �ݴ�� ��������Ѵ�.
        // rotateX�� ���� �ּ�,�ִ� ���� ���� ����Ѵ�.
        rotateX = Mathf.Clamp(rotateX + rotationY * -1f, limitRotateX.min, limitRotateX.max);
        camTransform.localRotation = Quaternion.Euler(rotateX, 0f, 0f);
    }

}
