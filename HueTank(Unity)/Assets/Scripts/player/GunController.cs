using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

	public Transform weaponHoldPoint;
	private Gun equippedGun;
	public Gun[] gunArsenal;
	public int currentGunIndex;


	void Start()
	{
		EquippGun (gunArsenal[currentGunIndex]);
	}

	// экипировать пушку
	public void EquippGun(Gun gunToEquip)
	{
		if (equippedGun != null) 
		{
			Destroy (equippedGun.gameObject);
		}
		equippedGun = Instantiate(gunToEquip, weaponHoldPoint.position, weaponHoldPoint.rotation);
		equippedGun.transform.parent = weaponHoldPoint;
	}

	// следующая (+1) / преддущая (-1) пушка
	public void ChangeWeapon(int changeIndex)
	{
		currentGunIndex += changeIndex;

		// верхняя граница
		if (currentGunIndex > gunArsenal.Length-1) 
		{
			currentGunIndex = 0;
		}
		// нижняя граница
		if (currentGunIndex < 0) 
		{
			currentGunIndex = gunArsenal.Length-1;
		}

		EquippGun (gunArsenal[currentGunIndex]);

		GetComponent<GUIManager> ().ChangeGUIgunActive ();
		SoundManager.instance.PlaySound (gunArsenal[currentGunIndex].activateSound, transform.position);
	}
		

	public void Shoot()
	{
		if (equippedGun != null) 
		{
			equippedGun.Shoot ();
		}
	}
}
