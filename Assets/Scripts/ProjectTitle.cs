using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTitle : MonoBehaviour
{
    float speed = 5.0f;

    public void SetSpeed(float newSpeed){
        speed = newSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate (Vector3.forward * Time.deltaTime * speed);
    }
}
