using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
    public int damage; // Sát thương của vũ khí
    public Camera camera; // Camera để xác định hướng bắn
    public float fireRate; // Tốc độ bắn
    private float nextFire;

    [Header("VFX")]
    public GameObject hitVFX; // Hiệu ứng khi bắn trúng

    void Update()
    {
        // Giảm thời gian đợi giữa các lần bắn
        if (nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        // Kiểm tra nếu chuột trái được nhấn và thời gian giữa các lần bắn đã đủ
        if (Input.GetMouseButton(0) && nextFire <= 0) // 0 là nút chuột trái
        {
            nextFire = 1 / fireRate;
            Fire();
        }
    }

    void Fire()
    {
        // Tạo ray từ camera hướng về phía trước
        if (camera == null)
        {
            Debug.LogError("Camera chưa được gán!");
            return;
        }

        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Tạo hiệu ứng trúng mục tiêu
            if (hitVFX != null)
            {
                PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);
            }

            // Gây sát thương nếu mục tiêu có thành phần Health
            var health = hit.transform.gameObject.GetComponent<Health>();
            var photonView = hit.transform.gameObject.GetComponent<PhotonView>();
            if (health != null && photonView != null)
            {
                photonView.RPC("TakeDamage", RpcTarget.All, damage);
            }
        }
    }
}
