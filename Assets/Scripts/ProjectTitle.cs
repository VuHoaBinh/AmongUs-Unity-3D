using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTitle : MonoBehaviour
{
    float speed = 5.0f;
    public LayerMask collisionMask;
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
        float moveDistance = Time.deltaTime * speed;
        transform.Translate (Vector3.forward * moveDistance );
        CheckCollisions(moveDistance);
    }

    void CheckCollisions(float moveDistance){
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit; 

        if (Physics.Raycast(ray, out hit, moveDistance + 0.5f, collisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit){
        print(hit.collider.gameObject.name);
        GameObject.Destroy(hit.collider.gameObject);
    }
}
