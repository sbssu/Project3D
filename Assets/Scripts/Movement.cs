using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float gravityScale;        // 중력 배율.
    [SerializeField] float jumpHeight;          // 점프 높이.
    [SerializeField] float movSpeed;            // 이동 속도.

    CharacterController controller;             // 컨트롤러.
    Vector3 velocity;                           // 속력.
    LayerMask groundMask;                       // 지면 마스크.
    bool isGrounded;                            // 땅에 서 있는가?

    float GRAVITY_VALUE => -9.81f * gravityScale;   // 실제 중력 값.

    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundMask = 1 << LayerMask.NameToLayer("Ground");

        Block block = BlockManager.Instance.GetBlock("block:dirt");
        block.transform.Translate(Vector3.left);
        block = BlockManager.Instance.GetBlock("block:coal");
        block.transform.Translate(Vector3.down);
        block = BlockManager.Instance.GetBlock("block:coal");
        block.transform.Translate(Vector3.right);
        block.transform.Translate(Vector3.down);
        block.transform.Translate(Vector3.down);
    }

    private void FixedUpdate()
    {
        // 물리적인 처리는 정확도 보장을 위해 고정 시간 기준으로 호출되는 FixedUpdate에서 구현.
        Gravity();
    }
    void Update()
    {
        UserInput();
        Jump();
    }

    private void Gravity()
    {
        // 지면에 닿았는지 체크. (최소한의 중력값을 적용해 바닥에 붙을 수 있도록 한다)
        isGrounded = Physics.CheckSphere(transform.position, 0.2f, groundMask);
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity += Vector3.up * GRAVITY_VALUE * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    private void UserInput()
    {
        float speed = 8;
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        Vector3 dir = (transform.forward * ver + transform.right * hor).normalized;
        controller.Move(dir * speed * Time.deltaTime);
    }
    private void Jump()
    {
        // 점프 키를 누르면 jumpHeight 높이까지 뛴다.
        if (Input.GetKeyDown(KeyCode.Space))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY_VALUE);
    }

}
