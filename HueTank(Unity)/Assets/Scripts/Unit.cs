using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// родительский класс игрока и всех противников
public class Unit : MonoBehaviour, IDamagable {

	public float hpStart; 
	public float armor;
	protected float hp;
	[HideInInspector]
	public bool isDead;

	public GameObject hitEffect;
	public AudioClip hitSound;
	public AudioClip dieSound;


	public virtual void Start()
	{
		hp = hpStart;
	}

	public virtual void Update()
	{

	}

	// получение урона 
	public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		SoundManager.instance.PlaySound (hitSound, transform.position);

		hp -= damage*armor;

		if (hp <= 0 && !isDead) 
		{
			Die ();
		} 
			
		HitPlaceEffect (hitPoint, hitDirection);
	}

	// эффект попадания
	public virtual void HitPlaceEffect(Vector3 hitPoint, Vector3 hitDirection)
	{
		GameObject newHitEffect = Instantiate (hitEffect, hitPoint, Quaternion.FromToRotation (Vector3.forward, hitDirection));
		Destroy (newHitEffect, 3f);
	}


	public virtual void Die()
	{
		isDead = true;
	}
}
