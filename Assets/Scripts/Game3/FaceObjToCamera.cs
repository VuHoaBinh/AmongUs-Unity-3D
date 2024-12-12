using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceObjToCamera : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
