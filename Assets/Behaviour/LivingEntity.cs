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

    public void TakeDamage(float damage){
        health -= damage;
        if(health <= 0 && !dead){
            Die();
        }
    }

    public void TakeHit(float damage, RaycastHit hit){
        TakeDamage(damage);
    }

    public void Die(){
        dead = true;
        GameObject.Destroy(gameObject);
        if(OnDeath!= null){
            OnDeath();
        }
        
    }
   
}
