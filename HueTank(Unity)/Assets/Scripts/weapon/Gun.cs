using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public string gunName;
	public Sprite gunImage;
	public Transform muzzle;
	public Projectile bullet;
	public int shotBulletAmount;
	public float bulletSpeed;
	public float bulletDamage;
	public float bulletDeltaY;
	public float bulletDeltaZ;
	public float msBetweenShots;
	public bool flyThroughEnemy;

	public Transform shell;
	public Transform shellBornPoint;

	public float screenShakeDuration;
	public float screenShakeMagnitude;

	public Transform muzzleFlash;

	private float nextShootTime;

	public AudioClip shootSound;
	public AudioClip activateSound;

	public void Shoot()
	{
		if (Time.time > nextShootTime)
		{
			nextShootTime = Time.time + msBetweenShots / 1000;


			CreateBullet ();	
			PlayMuzzleEffects ();

			StartCoroutine(Camera.main.GetComponent<ScreenShaker>().Shake(screenShakeDuration, screenShakeMagnitude));

			SoundManager.instance.PlaySound (shootSound, transform.position);
		}
	}

	public void CreateBullet()
	{
		for (int i = 0; i < shotBulletAmount; i++) 
		{
			Projectile newProjectile = Instantiate (bullet, muzzle.position, muzzle.rotation);
			newProjectile.speed = bulletSpeed * Random.Range(0.9f, 1.1f);
			newProjectile.damage = bulletDamage;
			newProjectile.deltaY = Random.Range (-bulletDeltaY, bulletDeltaY);
			newProjectile.deltaZ = Random.Range (-bulletDeltaZ, bulletDeltaZ);
			newProjectile.flyThroughEnemy = flyThroughEnemy;
		}
	}

	public void CreateShell()
	{
		Instantiate (shell, shellBornPoint.position, shellBornPoint.rotation);
	}

	public void PlayMuzzleEffects()
	{
		muzzleFlash.gameObject.SetActive (true);
		Invoke ("StopMuzzleEffects", 0.1f);
	}

	public void StopMuzzleEffects()
	{
		muzzleFlash.gameObject.SetActive (false);
	}

}
