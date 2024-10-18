using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent ( typeof (RigidBody))]
public class PlayerController : MonoBehaviour
{
     Rigidbody myRigidbody;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void LookAt(Vector3 lookPoint){
        Vector3 correctPoint=new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(correctPoint);
    }
    // Update is called once per frame
    void Update()
    {
        myRigidbody.MovePosition(myRigidbody.position+velocity*Time.deltaTime);
    }
}
