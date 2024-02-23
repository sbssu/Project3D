using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] LayerMask targetMask;      // Ž�� ��� ����ũ
    [SerializeField] float patrolRange;         // ���� ����.
    [SerializeField] float chaseRange;          // ���� ����.
    [SerializeField] float detectRange;         // Ž�� ����.
    [SerializeField] float absoluteRange;       // ���� ����.
    [SerializeField] float detectAngle;         // �þ� ��

    NavMeshAgent agent;
    Transform target;
    Vector3 spawnPosition;          // �¾ ��ġ.
    Vector3 patrolPoint;            // ���� ��ġ.
    Vector3 rayDirection;           // (Debug) Ÿ�� ����.
    float stayTime;                 // ��� �ð�.

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPosition = transform.position;
        patrolPoint = transform.position;    
        rayDirection = Vector3.zero;               
        target = null;                       
    }

    // Update is called once per frame
    void Update()
    {
        // ���� Ž���ϴ� ���� (Detect)
        if(target == null)
        {
            SetPatrolPoint();
            DetectTarget();
        }
        // ���� �߰��ϴ� ���� (Chase)
        else
        {
            agent.SetDestination(target.position);
            rayDirection = Vector3.zero;

            if (Vector3.Distance(spawnPosition, transform.position) > chaseRange)
                target = null;
        }
    }
    private void SetPatrolPoint()
    {
        if(!agent.hasPath)
        {
            if (stayTime > 0)
            {
                stayTime -= Time.deltaTime;
            }
            else
            {
                Vector2 randomCircle = Random.insideUnitCircle * patrolRange;
                patrolPoint = spawnPosition + new Vector3(randomCircle.x, 0f, randomCircle.y);
                stayTime = Random.Range(1.0f, 4.0f);
            }
        }

        agent.SetDestination(patrolPoint);
    }
    private void DetectTarget()
    {
        rayDirection = Vector3.zero;

        // ���� ���� �ȿ� Ÿ���� ������ ���
        Collider[] colliders = Physics.OverlapSphere(transform.position, absoluteRange, targetMask);
        if(colliders.Length > 0)
        {
            target = colliders[0].transform;
            return;
        }

        // �� ������ �浹 ���� ���� Ÿ���� �����Ǿ����� üũ.
        colliders = Physics.OverlapSphere(transform.position, detectRange, targetMask);
        if (colliders.Length <= 0)
            return;
                
        Transform tempTarget = colliders[0].transform;                      // �浹�� ��� 
        rayDirection = tempTarget.position - transform.position;            // ����� ���ϴ� ����.

        // �þ߰� ���.
        Vector3 me = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 you = new Vector3(tempTarget.position.x, 0f, tempTarget.position.z);
        float angle = Vector3.Angle(transform.forward, you - me);
        if (detectAngle * 0.5f < angle)
            return;

        // ����� ���� ���� �߻� : Ÿ�ٰ� �� ���� ���� �Ÿ��� ��ֹ��� ���� ��� (�þ�)
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, float.MaxValue))
        {
            if (hit.collider.transform == tempTarget)
                target = tempTarget;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, rayDirection);

        Quaternion rot = Quaternion.AngleAxis(detectAngle * 0.5f, Vector3.up);           // ȸ����.
        Vector3 movement = rot * (transform.forward * detectRange);                      // �̵���.
        Vector3 posA = transform.position + movement;                                    // �� A�� ��ġ.
        Vector3 posB = transform.position + Vector3.Reflect(movement, transform.right);  // �� B�� ��ġ. (�ݻ簢 �̿�)

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, posA);
        Gizmos.DrawLine(transform.position, posB);

        // �� B�κ��� detectRange�� �������� ���� ���� ȣ �׸���.
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, posB - transform.position, detectAngle, detectRange);

        // �̸� ��.
        GUIStyle style = new GUIStyle();
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.red;
        UnityEditor.Handles.Label(transform.position, transform.name, style);

        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, absoluteRange);
    }
    private void OnDrawGizmosSelected()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (target == null)
            {
                UnityEditor.Handles.color = Color.green;
                UnityEditor.Handles.DrawWireDisc(spawnPosition, Vector3.up, patrolRange);                                

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(patrolPoint, 1f);
            }
            else
            {
                UnityEditor.Handles.color = Color.red;
                UnityEditor.Handles.DrawWireDisc(spawnPosition, Vector3.up, chaseRange);                
            }
        }
        else
        {
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, patrolRange);
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, chaseRange);
         
        }
    }

}
