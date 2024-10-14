using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent ( typeof (PlayerController))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5;
    PlayerController controller;

    Camera viewCamera;

    GunController gunController;
    void Start()
    {
        controller = GetComponent<PlayerController>();
        viewCamera = Camera.main;
        gunController = GetComponent<GunController>();
    }

    void Update()
    {
        // movement input
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

        // look input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if( groundPlane.Raycast(ray, out rayDistance)){
            Vector3 point = ray.GetPoint(rayDistance);
            // Debug.DrawLine(ray.origin, point, Color.red);
            controller.LookAt(point);
        }

        // weapon input
        if (Input.GetMouseButtonDown(0)){
            gunController.Shoot();
        }


    }
}
