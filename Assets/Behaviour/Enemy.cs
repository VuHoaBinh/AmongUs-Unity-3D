using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public enum State{Idle,Chasing,Attacking};
    State currentState;

    NavMeshAgent pathfinder;
    Transform target; 
    LivingEntity targetEntity;
    Material skinMaterial;
    Color originalColor;

    float attackDistanceThreshold = 1.5f;
    float timeBetweenAttacks = 1;
    float damage=1;

    float nextAttackTime;
    float myCollsionRadius;
    float targetCollsionRadius;

    bool hasTarget;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();  
        pathfinder=GetComponent<NavMeshAgent>(); 
        skinMaterial=GetComponent<Renderer>().material;
        originalColor=skinMaterial.color;

        if(GameObject.FindGameObjectWithTag("Player")!=null){
            currentState=State.Chasing;
            hasTarget= true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity= target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollsionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollsionRadius= target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePosition());
        }
        
    }

    void OnTargetDeath(){
        hasTarget=false;
        currentState=State.Idle;
        pathfinder.enabled=true;
    }

    public void Update(){
        
        if(hasTarget){
            if(Time.time> nextAttackTime) {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if(sqrDstToTarget< Mathf.Pow(attackDistanceThreshold+myCollsionRadius+targetCollsionRadius,2)){
                    nextAttackTime=Time.time+timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
    }
    IEnumerator Attack(){
        currentState=State.Attacking;
        pathfinder.enabled=false;

        Vector3 originalPosition= transform.position;
        Vector3 dirToTarget = (target.position-transform.position).normalized;
        Vector3 attackPosition = target.position-dirToTarget*(myCollsionRadius+targetCollsionRadius);

        float percent=0;
        float attackSpeed=3;
        skinMaterial.color=Color.red;
        bool hasAppliedDamage = false;

        while(percent<=1){
            if(!hasAppliedDamage && percent>=0.5f){
                hasAppliedDamage=true;
                targetEntity.TakeDamage(damage);
            }
            percent+=Time.deltaTime*attackSpeed;
            float interpolation=(-Mathf.Pow(percent,2) +percent)*4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            yield return null;
        }
        skinMaterial.color=originalColor;   
        currentState= State.Chasing;
        pathfinder.enabled=true;
    }

    IEnumerator UpdatePosition(){
        float refreshRate=0.25f;
        while (hasTarget){
            if(currentState== State.Chasing){
                Vector3 dirToTarget = (target.position-transform.position).normalized;
                Vector3 targetPosition = target.position-dirToTarget*(myCollsionRadius+targetCollsionRadius+attackDistanceThreshold/2);
                if(!dead) {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
        
}
