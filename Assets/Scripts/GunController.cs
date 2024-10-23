using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    Gun equippedGun;
    public Transform weaponHold;
    public Gun startingGun;
    // Start is called before the first frame update

    void Start(){
        if (startingGun != null){
            EquipGun(startingGun);
        } 
    }
    public void EquipGun(Gun gunToEquip){
        if (equippedGun != null){
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip, weaponHold.position, weaponHold.rotation) as Gun;
        equippedGun.transform.parent = weaponHold;
    }

    public void OnTriggerHold(){
        if (equippedGun != null){
            equippedGun.OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
            equippedGun.OnTriggerRelease();
        }
    }
}
