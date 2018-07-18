using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {
	
	public GameObject hitObstacleEffect;
	Transform camera;

	Animator animator;
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

	void Start()
	{
		camera = Camera.main.transform;
		animator = GetComponent<Animator> ();
	}

	void Update()
	{
		Debug.DrawRay(camera.transform.position, camera.transform.forward*100f, Color.green);
	}

	public void RpcShoot()
	{
		Shoot ();
	}

	public void Shoot()
	{
		
		if (Time.time > nextShootTime)
		{

			nextShootTime = Time.time + msBetweenShots / 1000;

			animator.Play ("FIRE");

			PlayMuzzleEffects ();
			CreateShell ();

			Ray ray = new Ray (camera.transform.position, camera.transform.forward);
			RaycastHit hit;

			if (Physics.Raycast (ray, out hit, 500f)) 
			{

				if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Enemies")) 
				{
					hit.collider.gameObject.GetComponent<Enemy>().TakeHit (1f, hit.point, ray.direction);
				}


				if (hit.collider.gameObject.layer == LayerMask.NameToLayer ("Obstacles")) 
				{
					Destroy (Instantiate (hitObstacleEffect, hit.point, transform.rotation) as GameObject, 3f);
				}
			}
		}
	}

	public void CreateBullet()
	{
		Projectile newProjectile = Instantiate (bullet, muzzle.position, muzzle.rotation);
		newProjectile.speed = bulletSpeed;
		newProjectile.damage = bulletDamage;
		newProjectile.deltaY = Random.Range (-bulletDeltaY, bulletDeltaY);
		newProjectile.deltaZ = Random.Range (-bulletDeltaZ, bulletDeltaZ);
	}

	public void CreateShell()
	{
		Instantiate (shell, shellBornPoint.position, shellBornPoint.rotation);
	}

	public void PlayMuzzleEffects()
	{
		muzzleFlash.Activate ();
	}



}
