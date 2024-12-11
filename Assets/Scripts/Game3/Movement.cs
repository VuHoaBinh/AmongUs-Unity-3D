using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float maxVelocityChange = 10f;

    private Vector2 input;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Sử dụng Input.GetAxisRaw từ Unity
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize(); // Chuẩn hóa vector để đảm bảo độ dài là 1
    }

    // FixedUpdate là phương thức chính xác để làm việc với Rigidbody
    void FixedUpdate()
    {
        rb.AddForce(CalculateMovement(walkSpeed), ForceMode.VelocityChange);
    }

    // Phương thức tính toán chuyển động
    Vector3 CalculateMovement(float _speed)
    {
        // Tính toán vận tốc mục tiêu
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= _speed;

        // Lấy vận tốc hiện tại
        Vector3 velocity = rb.velocity;

        if (input.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - velocity;

            // Giới hạn thay đổi vận tốc theo trục X và Z
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);

            velocityChange.y = 0; // Không thay đổi vận tốc theo trục Y
            return velocityChange;
        }
        else
        {
            return Vector3.zero; // Trả về Vector3.zero khi không có input
        }
    }
}
