using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class TouchPlayer : MonoBehaviour
{
    LayerMask groundMask;
    NavMeshAgent agent;
    Camera cam;

    void Start()
    {
        groundMask = 1 << LayerMask.NameToLayer("Ground");
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
    }
    Vector3 point;

    private void Update()
    {
        // ���콺 ������ (=��ũ�� �����̽�)�� ��ġ���� ī�޶� ���� �������� ���̸� �߻�.
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, groundMask))
            {
                point = hit.point;
                agent.SetDestination(point);
            }
            else
            {
                point = ray.origin;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(point, 0.2f);
    }

}
