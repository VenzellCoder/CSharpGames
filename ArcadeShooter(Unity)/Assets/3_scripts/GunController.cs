using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

	public Transform weaponHoldPoint;
	private Gun equippedGun;
	public Gun starterGun;

	void Start()
	{
		EquippGun (starterGun);
	}

	public void EquippGun(Gun gunToEquip)
	{
		if (equippedGun != null) 
		{
			Destroy (equippedGun.gameObject);
		}
		equippedGun = Instantiate(gunToEquip, weaponHoldPoint.position, weaponHoldPoint.rotation);
		equippedGun.transform.Rotate (0, 0, 90);
		equippedGun.transform.parent = weaponHoldPoint;
	}

	public void Shoot()
	{
		if (equippedGun != null) 
		{
			equippedGun.Shoot ();
		}
	}

	public float GetGunHeight()
	{
		return weaponHoldPoint.position.y;
	}
}
