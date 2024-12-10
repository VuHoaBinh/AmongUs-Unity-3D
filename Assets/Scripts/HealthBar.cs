using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Slider easeHeathSlider;
    public float maxHealth=5f;
    public float health;
    private float lerpSpeed= 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        health=maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthSlider.value != health){
            healthSlider.value = health;
        }
        
        if(healthSlider.value != easeHeathSlider.value){
            easeHeathSlider.value = Mathf.Lerp(easeHeathSlider.value, health, lerpSpeed);
        }
        
    }
    public void TakeDamage(float damage){
        health -= damage;

    }
}
