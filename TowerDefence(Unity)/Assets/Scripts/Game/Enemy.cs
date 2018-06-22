using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	[Header("UI elements")]
	public Image armorBar;
	public Image hpBar;

	[Header("Properties")]
	public int hpStart ;
	public int armorStart;
	public int moneyForKill;
	public int damage;
	public float speed;

	private int hp;
	private int armor;
	private Transform target;
	private int pathIndex = 0;
	private PlaySessionManager playSessionManager;


	void Start () 
	{
		playSessionManager = GameObject.Find ("PlaySessionManager").GetComponent<PlaySessionManager> ();

		armor = armorStart;
		hp = hpStart;
		target = Path.points [pathIndex];
	}

	// Двигаться к текущей точки пути
	void Update () 
	{
		Vector3 direction = target.position - transform.position;

		transform.Translate (direction.normalized * speed * Time.deltaTime, Space.World);

		if (Vector3.Distance (transform.position, target.position) < 0.2f) 
		{
			GetNextTarget ();
		}
		/*
		// угол к цели в радианах
		float angRad = Mathf.Atan2 (target.position.y - transform.position.y, target.position.x - transform.position.x);
		// угол к цели в градусах
		float andDeg = (180 / Mathf.PI) * angRad;
		// мгновенное изменение угла поворота по Z
		//transform.rotation = Quaternion.Euler (0, 0, andDeg);
		// плавное изменение угла поворота по Z
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler (0, 0, andDeg), Time.deltaTime * 5f);
		*/
	}

	void GetNextTarget()
	{
		if (pathIndex >= Path.points.Length-1) 
		{
			Destroy (gameObject);
			playSessionManager.TakeDamage (damage);
			return;
		}

		pathIndex++;
		target = Path.points [pathIndex];
	}

	// сначала урон убирает броню, потом жизни
	public void TakeDamage(int _damage)
	{
		if (armor > 0) 
		{
			if (_damage > armor) 
			{
				armor -= _damage;
				hp -= (_damage - armor);
			} 
			else
			{
				armor -= _damage;
			}
		} 
		else 
		{
			armor = 0;
			hp -= _damage;
		}

		// шкалы брони и жизни
		armorBar.fillAmount = (float)armor / (float)armorStart;
		hpBar.fillAmount = (float)hp / (float)hpStart;


		if (hp <= 0) 
		{
			// доход с убийства
			playSessionManager.ChangeMoney (+moneyForKill);
			// статистика 
			playSessionManager.statKills++;
			Destroy (gameObject);
		}
	}
}
