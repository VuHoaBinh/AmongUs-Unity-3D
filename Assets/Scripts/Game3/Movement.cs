using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float sprintSpeed = 14f;
    public float maxVelocityChange = 10f;

    [Space]
    public float airControl = 0.5f;

    [Space]
    public float jumpHeight = 5f;

    private Vector2 input;
    private Rigidbody rb;
    private bool sprinting;
    private bool jumping;
    private bool grounded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Ngăn nhân vật bị xoay không mong muốn
    }

    void Update()
    {
        // Nhận input từ người chơi
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();

        sprinting = Input.GetKey(KeyCode.LeftShift); // Sử dụng Left Shift để chạy
        jumping = Input.GetKey(KeyCode.Space); // Sử dụng Space để nhảy
    }

    private void OnCollisionStay(Collision collision)
    {
        // Kiểm tra va chạm với mặt đất
        grounded = true;
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            // Xử lý nhảy
            if (jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, CalculateJumpVelocity(), rb.velocity.z);
            }
            else
            {
                // Xử lý di chuyển
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed : walkSpeed), ForceMode.VelocityChange);
            }
        }
        else
        {
            rb.AddForce(CalculateMovement(sprinting ? sprintSpeed*airControl : walkSpeed*airControl), ForceMode.VelocityChange);

        }

        // Reset trạng thái grounded để kiểm tra lại trong khung hình tiếp theo
        grounded = false;
    }

    Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= _speed;

        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0, velocity.z);

        // Giới hạn thay đổi vận tốc
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;

        return velocityChange;
    }

    float CalculateJumpVelocity()
    {
        // Tính vận tốc cần thiết để đạt độ cao nhảy
        return Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude);
    }
}
