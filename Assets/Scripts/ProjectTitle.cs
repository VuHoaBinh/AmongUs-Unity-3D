using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTitle : MonoBehaviour
{
    public LayerMask collisionMask;
    float speed =10;
    float damage =1;
    public float life=2;
    float skinWidth =.1f;
    public Color trailColor;

    // Start is called before the first frame update
    
    void Start(){
        GameObject.Destroy(gameObject, life);
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        if (initialCollisions.Length>0){
            OnHitObject(initialCollisions[0], transform.position);
        }
        GetComponent<TrailRenderer>().material.SetColor("_Color", trailColor);

    } 
    public void SetSpeed(float newSpeed){
        speed = newSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        float moveDistance = speed*Time.deltaTime;
        CheckCollision(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);

    }
    void CheckCollision(float moveDistance){
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide)){
            OnHitObject(hit.collider, hit.point);
        }
    }
    // void OnHitObject(RaycastHit hit){
    //     IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
    //     if(damageableObject != null){
    //         damageableObject.TakeHit(damage, hit);
    //     }
    //     GameObject.Destroy(gameObject);
    // }

    void OnHitObject(Collider c,  Vector3 hitPoint){
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if(damageableObject != null){
            damageableObject.TakeHit(damage, hitPoint ,transform.forward);
        }
        GameObject.Destroy(gameObject);
    }
}
