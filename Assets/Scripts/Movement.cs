using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float gravityScale;        // �߷� ����.
    [SerializeField] float jumpHeight;          // ���� ����.
    [SerializeField] float movSpeed;            // �̵� �ӵ�.

    CharacterController controller;             // ��Ʈ�ѷ�.
    Vector3 velocity;                           // �ӷ�.
    LayerMask groundMask;                       // ���� ����ũ.
    bool isGrounded;                            // ���� �� �ִ°�?

    float GRAVITY_VALUE => -9.81f * gravityScale;   // ���� �߷� ��.

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
        // �������� ó���� ��Ȯ�� ������ ���� ���� �ð� �������� ȣ��Ǵ� FixedUpdate���� ����.
        Gravity();
    }
    void Update()
    {
        UserInput();
        Jump();
    }

    private void Gravity()
    {
        // ���鿡 ��Ҵ��� üũ. (�ּ����� �߷°��� ������ �ٴڿ� ���� �� �ֵ��� �Ѵ�)
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
        // ���� Ű�� ������ jumpHeight ���̱��� �ڴ�.
        if (Input.GetKeyDown(KeyCode.Space))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY_VALUE);
    }

}
