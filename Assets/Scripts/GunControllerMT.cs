using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControllerMT : MonoBehaviour
{
    public Transform weaponHold;
	public GunMT[] allGuns;
	GunMT equippedGun;

	void Start() {
        EquipGun(0);
	}

	public void EquipGun(GunMT gunToEquip) {
		if (equippedGun != null) {
			Destroy(equippedGun.gameObject);
		}
		equippedGun = Instantiate (gunToEquip, weaponHold.position,weaponHold.rotation) as GunMT;
		equippedGun.transform.parent = weaponHold;
	}

	public void EquipGun(int weaponIndex){
		EquipGun(allGuns[weaponIndex]);
	}

	public void OnTriggerHold() {
		if (equippedGun != null) {
			equippedGun.OnTriggerHold();
		}
	}

	public void OnTriggerRelease() {
		if (equippedGun != null) {
			equippedGun.OnTriggerRelease();
		}
	}

	public void Reload() {
		if (equippedGun != null) {
			equippedGun.Reload();
		}
	}

}
