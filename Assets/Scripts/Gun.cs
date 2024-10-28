using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum FireMode{Auto, Burst, Single};
    public FireMode fireMode;
    public Transform[] projecttileSpawn;
    public ProjectTitle projectTitle;
    public float msBetweenShots = 100;
    public float muzzleVelocity = 35;
    public int burstCount;
    public Transform shell;
    public Transform shellEjection;
    MuzzleFlash muzzleFlash;
    float nextShotTime;

    bool triggerReleaseSinceLastShot;
    int shotsRemainingInBurst;

    Vector3 recoilSmoothDampVelocity;
    float recoilAngle;
    float recoilRotSmoothDampVelocity;
    void Start(){
        muzzleFlash = GetComponent<MuzzleFlash>();
        shotsRemainingInBurst  = burstCount;
    }

    void LaterUpdate(){
        // animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, .1f);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, Vector3.zero.x, ref recoilRotSmoothDampVelocity, .1f);
        transform.localEulerAngles += Vector3.left * recoilAngle;
    }
    void Shoot()
    {
        if(Time.time > nextShotTime){
            if (fireMode == FireMode.Burst){
                if(shotsRemainingInBurst == 0){
                    return;
                }
                shotsRemainingInBurst--;
            }else if(fireMode == FireMode.Single){
                 if(!triggerReleaseSinceLastShot){
                    return;
                }
            }

            for (int i = 0; i < projecttileSpawn.Length; i++)
            {
                nextShotTime = Time.time + msBetweenShots / 1000;
                ProjectTitle newProjectile = Instantiate(projectTitle, projecttileSpawn[i].position, projecttileSpawn[i].rotation) as ProjectTitle;
                newProjectile.SetSpeed(muzzleVelocity);
                // Destroy(newProjectile, 5f);

                
            }
            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();
            transform.localPosition = Vector3.forward * .2f;
            recoilAngle += 5;
			recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
        }
    }

    public void Aim(Vector3 aimPoint)
    {
       transform.LookAt (aimPoint);
    }
    public void OnTriggerHold(){
        Shoot();
        triggerReleaseSinceLastShot = false;
    }

    public void OnTriggerRelease(){
        triggerReleaseSinceLastShot = true;
        shotsRemainingInBurst  = burstCount;
    }
}
