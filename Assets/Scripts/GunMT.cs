using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMT : MonoBehaviour
{
    public enum FireMode{Auto, Burst, Single};
    public FireMode fireMode;
    public Transform[] projecttileSpawn;
    public ProjectTitle projectTitle;
    public float msBetweenShots = 25;
    public float muzzleVelocity = 35;
    public int burstCount;
    public int projectilePerMag;
    public float reloadTime = .5f;

    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(.05f,.2f);
    public Vector2 recoilAngleMinMax = new Vector2(3,5);
    public float recoilMoveSettleSpeed = .1f;
    public float recoilRotationSettleSpeed = .1f;


    [Header("Effects")]
    public Transform shell;
    public Transform shellEjection;
    // MuzzleFlash muzzleFlash;
    float nextShotTime;
    public AudioClip shootAudio;
    public AudioClip reloadAudio;


    public Camera playerCamera;

    bool triggerReleaseSinceLastShot;
    int shotsRemainingInBurst;
    int projectileRemainingInMag;
    bool isReloading;

    Vector3 recoilSmoothDampVelocity;
    float recoilAngle;
    float recoilRotSmoothDampVelocity;
    void Start(){
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        // muzzleFlash = GetComponent<MuzzleFlash>();
        shotsRemainingInBurst  = burstCount;
        projectileRemainingInMag = projectilePerMag;
    }
    // void Start() {
    //     if (projecttileSpawn == null || projecttileSpawn.Length == 0) {
    //         Debug.LogError("projecttileSpawn is not assigned!");
    //     }
    //     if (projectTitle == null) {
    //         Debug.LogError("projectTitle is not assigned!");
    //     }
    //     // if (muzzleFlash == null) {
    //     //     Debug.LogError("muzzleFlash is not assigned!");
    //     // }
    //     if (shell == null) {
    //         Debug.LogError("shell is not assigned!");
    //     }
    // }


    void LaterUpdate(){
  
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleSpeed);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleSpeed);
        transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;

        if(!isReloading && projectileRemainingInMag == 0){
            Reload();
        }

    }
    void Shoot()
    {
        if (!isReloading && Time.time > nextShotTime && projectileRemainingInMag > 0)
        {
            if (fireMode == FireMode.Burst)
            {
                if (shotsRemainingInBurst == 0)
                {
                    return;
                }
                shotsRemainingInBurst--;
            }
            else if (fireMode == FireMode.Single)
            {
                if (!triggerReleaseSinceLastShot)
                {
                    return;
                }
            }

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(1000); 
            }
            Vector3 shootDirection = (targetPoint - projecttileSpawn[0].position).normalized;

            for (int i = 0; i < projecttileSpawn.Length; i++)
            {
                if (projectileRemainingInMag == 0)
                {
                    break;
                }
                projectileRemainingInMag--;
                nextShotTime = Time.time + msBetweenShots / 1000;
                ProjectTitle newProjectile = Instantiate(projectTitle, projecttileSpawn[i].position, Quaternion.LookRotation(shootDirection)) as ProjectTitle;
                newProjectile.SetSpeed(muzzleVelocity);
            }

            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            // muzzleFlash.Activate();

            transform.localPosition = Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
            recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);

            // AudioManager.instance.PlaySound(shootAudio, transform.position);
        }
    }

    public void Reload(){
        if(!isReloading && projectileRemainingInMag != projectilePerMag){
            StartCoroutine(AnimateReload());
            AudioManager.instance.PlaySound(reloadAudio, transform.position);
        }
    }

    IEnumerator AnimateReload(){
        isReloading = true;
        yield return new WaitForSeconds(.2f);

        float reloadSpeed = 1 / reloadTime;
        float percent = 0;
        Vector3 initialRot = transform.localEulerAngles;

        float maxReloadAngle = 30;

        while (percent < 1){
            percent += Time.deltaTime * reloadSpeed;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);


            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

        
            yield return null;
        }


        isReloading = false;
        projectileRemainingInMag = projectilePerMag;

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
