using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform muzzle;
    public ProjectTitle projectTitle;
    public float msBetweenShots = 100;
    public float muzzleVelocity = 35;
    float nextShotTime;

    public void Shoot()
    {
        if(Time.time > nextShotTime){
            nextShotTime = Time.time + msBetweenShots / 1000;
            ProjectTitle newProjectile = Instantiate(projectTitle, muzzle.position, muzzle.rotation) as ProjectTitle;
            newProjectile.SetSpeed(muzzleVelocity);
            Destroy(newProjectile, 5f);
        }
    }
}
