using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRolation=0f;

    public float xSensitivity =30f;
    public float ySensitivity =30f;

    public void ProcessLook(Vector2 input){
        float mouseX = input.x;
        float mouseY = input.y;

        xRolation -=(mouseY*Time.deltaTime)*ySensitivity;
        xRolation = Mathf.Clamp(xRolation,-80f,80f);
        cam.transform.localRotation =Quaternion.Euler(xRolation,0,0);
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }

}
