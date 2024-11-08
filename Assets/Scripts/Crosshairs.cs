using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshairs : MonoBehaviour
{
    public LayerMask targetMask;

    public SpriteRenderer dot;
    public Color dotHighlightColor;

    Color originalDotColor;

    void Start()
    {
        Cursor.visible = false;
        originalDotColor = dot.color;
    }
    void Update()
    {
        transform.Rotate(Vector3.forward * 20 * Time.deltaTime);
    }

    public void DetectTargets(Ray ray){

        if(Physics.Raycast(ray, 100, targetMask)){
            dot.color = dotHighlightColor;
        }else{
            dot.color = originalDotColor;
        }

    }
}
