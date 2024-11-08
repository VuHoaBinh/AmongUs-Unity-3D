using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{

    public Sprite[] flashSprites;
    public SpriteRenderer[] flashRenderers; 
    public GameObject flashHolder;
    public float flashTime;

    void Start(){
        Deactivate();
    }
    public void Activate(){

        flashHolder.SetActive(true);

        int spriteIndex = Random.Range(0, flashSprites.Length);

        for(int i = 0; i < flashRenderers.Length; i++){

            flashRenderers[i].sprite = flashSprites[spriteIndex];

        }

        Invoke("Deactivate", flashTime);

    }

    public void Deactivate(){

        flashHolder.SetActive(false);
    }
}
