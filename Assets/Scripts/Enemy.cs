using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] LayerMask targetMask;      // 탐지 대상 마스크
    [SerializeField] float patrolRange;         // 정찰 범위.
    [SerializeField] float chaseRange;          // 추적 범위.
    [SerializeField] float detectRange;         // 탐지 범위.
    [SerializeField] float absoluteRange;       // 절대 범위.
    [SerializeField] float detectAngle;         // 시야 각

    NavMeshAgent agent;
    Transform target;
    Vector3 spawnPosition;          // 태어난 위치.
    Vector3 patrolPoint;            // 정찰 위치.
    Vector3 rayDirection;           // (Debug) 타겟 방향.
    float stayTime;                 // 대기 시간.

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
        // 적을 탐지하는 상태 (Detect)
        if(target == null)
        {
            SetPatrolPoint();
            DetectTarget();
        }
        // 적을 추격하는 상태 (Chase)
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

        // 절대 범위 안에 타겟이 존재할 경우
        Collider[] colliders = Physics.OverlapSphere(transform.position, absoluteRange, targetMask);
        if(colliders.Length > 0)
        {
            target = colliders[0].transform;
            return;
        }

        // 구 형태의 충돌 범위 내에 타겟이 감지되었는지 체크.
        colliders = Physics.OverlapSphere(transform.position, detectRange, targetMask);
        if (colliders.Length <= 0)
            return;
                
        Transform tempTarget = colliders[0].transform;                      // 충돌된 대상 
        rayDirection = tempTarget.position - transform.position;            // 대상을 향하는 방향.

        // 시야각 계산.
        Vector3 me = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 you = new Vector3(tempTarget.position.x, 0f, tempTarget.position.z);
        float angle = Vector3.Angle(transform.forward, you - me);
        if (detectAngle * 0.5f < angle)
            return;

        // 대상을 향해 레이 발사 : 타겟과 나 사이 직선 거리에 장애물이 없을 경우 (시야)
        if (Physics.Raycast(transform.position, rayDirection, out RaycastHit hit, float.MaxValue))
        {
            if (hit.collider.transform == tempTarget)
                target = tempTarget;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, rayDirection);

        Quaternion rot = Quaternion.AngleAxis(detectAngle * 0.5f, Vector3.up);           // 회전량.
        Vector3 movement = rot * (transform.forward * detectRange);                      // 이동량.
        Vector3 posA = transform.position + movement;                                    // 점 A의 위치.
        Vector3 posB = transform.position + Vector3.Reflect(movement, transform.right);  // 점 B의 위치. (반사각 이용)

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, posA);
        Gizmos.DrawLine(transform.position, posB);

        // 점 B로부터 detectRange의 반지름을 갖는 원의 호 그리기.
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireArc(transform.position, Vector3.up, posB - transform.position, detectAngle, detectRange);

        // 이름 라벨.
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
