using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Health : MonoBehaviour
{
    public int health;

    public TextMeshProUGUI healthText;
    public void TakeDamage(int _damage){
        health -=_damage;

        healthText.text = health.ToString();
        if(health <=0){
            Destroy(gameObject);
        }
    }
}
