using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Pun.UtilityScripts;

public class Weapon : MonoBehaviour
{
    public int damage;
    public Camera camera;
    public float fireRate;

    [Header("VFX")]
    public GameObject hitVFX;
    private float nextFire;

    [Header("Ammo")]

    public int mag = 5;
    public int ammo = 30;
    public int magAmmo = 30;

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("Animation")]
    public Animation animation;
    public AnimationClip reload;


    [Header("Recoil Settings")]
    // [Range(0, 1)]
    // public float recoilPercent = 0.3f;
    [Range(0, 2)]
    public float recoverPercent = 0.7f;

    [Space]
    public float recoilUp = 0f;
    public float recoilBack = 1f;

    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private float recoilLength;
    private float recoverLength;
    private bool recoiling;
    public bool recovering;
    void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;

        originalPosition = transform.localPosition;

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;

    }
    void Update()
    {
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        if (Input.GetMouseButton(0) && nextFire <= 0 && ammo > 0 && animation.isPlaying == false)
        {
            nextFire = 1 / fireRate;
            ammo--;
            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;
            Fire();
        }

        if (Input.GetKeyDown(KeyCode.R) && mag > 0)
        {
            Reload();
        }

        if (recoiling)
        {
            Recoil();
        }

        if (recovering)
        {
            Recover();
        }
    }

    void Reload()
    {
        animation.Play(reload.name);
        if (mag > 0)
        {
            mag--;
            ammo = magAmmo;
        }
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;

    }
    void Fire()
    {

        recoiling = true;
        recovering = false;

        if (camera == null)
        {
            Debug.LogError("Camera chưa được gán!");
            return;
        }

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;
        PhotonNetwork.LocalPlayer.AddScore(1);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            PhotonNetwork.LocalPlayer.AddScore(damage);

            if (hitVFX != null)
            {
                PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
            }

            var health = hit.transform.gameObject.GetComponent<Health>();
            if (health != null)
            {
                var photonView = hit.transform.gameObject.GetComponent<PhotonView>();
                if (photonView != null)
                {
                    photonView.RPC("TakeDamage", RpcTarget.All, damage);
                    if (damage >= health.health && RoomManager.instance != null)
                    {
                        RoomManager.instance.kills++;
                        RoomManager.instance.SetHashes();
                        PhotonNetwork.LocalPlayer.AddScore(100);
                    }
                }
            }
        }
    }

    void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPosition.x, originalPosition.y + recoilUp, originalPosition.z - recoilBack);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);
        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }

    void Recover()
    {
        Vector3 finalPosition = originalPosition;
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);
        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }
}
