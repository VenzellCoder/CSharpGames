using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public Transform muzzle;
	public Projectile bullet;
	public float bulletSpeed;
	public float bulletDamage;
	public float bulletDeltaY = 2f;
	public float bulletDeltaZ = 2f;
	public float msBetweenShots = 100;

	public Transform shell;
	public Transform shellBornPoint;

	public MuzzleFlash muzzleFlash;

	private float nextShootTime;



	public void Shoot()
	{
		if (Time.time > nextShootTime)
		{
		muzzleFlash.Activate ();

		nextShootTime = Time.time + msBetweenShots / 1000;
		Projectile newProjectile = Instantiate (bullet, muzzle.position, muzzle.rotation);
		newProjectile.speed = bulletSpeed;
		newProjectile.damage = bulletDamage;
		newProjectile.deltaY = Random.Range (-bulletDeltaY, bulletDeltaY);
		newProjectile.deltaZ = Random.Range (-bulletDeltaZ, bulletDeltaZ);

		Instantiate (shell, shellBornPoint.position, shellBornPoint.rotation);
		}
	}
}
