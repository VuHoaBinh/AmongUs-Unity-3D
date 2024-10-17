using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent ( typeof (RigidBody))]
public class PlayerController : MonoBehaviour
{
    Vector3 velocity;
    Rigidbody  myRigidBody;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody >();
    }

    public void LookAt(Vector3 lookPoint){
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x,transform.position.y, lookPoint.z);
        // transform.LookAt(lookPoint);
        transform.LookAt(heightCorrectedPoint);

    }
    public void Move(Vector3 _velocity){
        velocity = _velocity;
    }
    void FixedUpdate(){
        myRigidBody.MovePosition(myRigidBody.position + velocity* Time.fixedDeltaTime);
    }
}
