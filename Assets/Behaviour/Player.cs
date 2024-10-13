using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
    public float moveSpeed=5;

    Camera viewCamera;
    PlayerController controller;
    GunController gunController;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        controller=GetComponent<PlayerController>();
        viewCamera=Camera.main;
        gunController=GetComponentInChildren<GunController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity=moveInput.normalized*moveSpeed;
        controller.Move(moveVelocity);

        Ray ray=viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane=new Plane(Vector3.up,Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance)){
            Vector3 point = ray.GetPoint(rayDistance);
            // Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
            
        }
        if(Input.GetMouseButton(0)){
            gunController.Shoot();

        }
    }
}
