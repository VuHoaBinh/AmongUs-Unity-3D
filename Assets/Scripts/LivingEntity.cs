using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviour, IDamageable
{
    protected float health;
    protected bool dead;
    public float startingHealth=10;
    public event System.Action OnDeath;
    public float GetHealth(){
        return health;
    }
    protected virtual void Start()
    { 
        health = startingHealth;
    }

    // override
    public virtual void TakeDamage(float damage){
        health -= damage;
        if(health <= 0 && !dead){
            Die();
        }
    }


    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection){
        TakeDamage(damage);
    }

    [ContextMenu("Self Destruct")]   
    protected void Die(){
        dead = true;
        if(OnDeath!= null){
            OnDeath();
        }
        GameObject.Destroy(gameObject);
        
    }
}
