using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour {


	[Header("UI elements")]
	public GameObject UIselected;
	public GameObject UIfireRange;
	public GameObject UIbuttonSell;
	public Bullet bulletPrefab;

	[Header("properties")]
	public float fireRate;
	public int bulletDamage;
	public float bulletSpeed;
	public float range;
	public int priceBuy;
	public int priceSell;

	private Transform target;
	private float fireCountdown = 0f;
	private bool selected = false;

	private PlaySessionManager playSessionManager;


	void Start () 
	{
		playSessionManager = GameObject.Find ("PlaySessionManager").GetComponent<PlaySessionManager> ();
		// поиск ближайшего врага каждые 0,5 секунды
		InvokeRepeating ("UpdateTarget", 0f, 0.5f);
		// скрыть GUI башни
		UIselected.SetActive (false);
		// редактирование размерв GUI дальности стрельбы
		UIfireRange.transform.localScale = new Vector3 (range, range, 0f);
	}


	void Update () 
	{
		// выбор башни, GUI продажи и дальности стрельбы
		// (!!!) TODO: Обработка нажатий на GUI через Ray
		if (Input.GetMouseButtonDown (0)) 
		{
			Vector3 cursorePlace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			cursorePlace.z = transform.position.z;

			if (Vector3.Distance(cursorePlace, transform.position) < 0.5f)
			{
				UIselected.SetActive (true);
			}
			else
			{
				cursorePlace.z = UIbuttonSell.transform.position.z;
				if (Vector3.Distance (cursorePlace, UIbuttonSell.transform.position) < 0.5f) 
				{
					Sell ();
				}
				UIselected.SetActive (false);
			}
		}

		// стрельба по врагу
		if (target == null)
			return;
		
		if (fireCountdown <= 0) 
		{
			Shoot ();
			fireCountdown = 1f / fireRate;
		}
		fireCountdown -= Time.deltaTime; 

	}

	// поиск ближайшего врага в радиусе стрельбы 
	void UpdateTarget()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("enemy");

		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;

		foreach (GameObject enemy in enemies) 
		{
			float distanceToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance) 
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
			if (nearestEnemy != null && shortestDistance < range) 
			{
				target = nearestEnemy.transform;
			} 
			else 
			{
				target = null;
			}
		}
	}

	void Shoot()
	{
		Bullet newBullet = Instantiate (bulletPrefab, transform.position, transform.rotation);
		newBullet.target = target;
		newBullet.speed = bulletSpeed;
		newBullet.damage = bulletDamage;
	}

	public void Sell()
	{
		playSessionManager.ChangeMoney (+priceSell);
		Destroy (gameObject);
	}
}
