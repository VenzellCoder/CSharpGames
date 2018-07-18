using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Unit : MonoBehaviour, IDamagable {

	
	public float hpStart; 

	protected float hp;
	protected bool isDead;

	public Color liveColour;
	public Color deadColour;
	public Color hitColour;
	public float colourLerpDurationDie;
	private float colourLerpCounterDie = 0f;
	public Transform materialHolderBody;
	public Transform materialHolderHead;

	public GameObject hitEffect;

	public Material bludMaterial;

	public virtual void Start()
	{
		hp = hpStart;

		if (materialHolderBody != null) 
		{
			materialHolderBody.GetComponent<Renderer> ().material.color = liveColour;
		}
		if (materialHolderHead != null) 
		{
			materialHolderHead.GetComponent<Renderer> ().material.color = liveColour;
		}
	}

	public virtual void Update()
	{
		if (isDead) 
		{
			
			materialHolderBody.GetComponent<Renderer> ().material.color = Color.Lerp (liveColour, deadColour, colourLerpCounterDie);
			materialHolderHead.GetComponent<Renderer> ().material.color = Color.Lerp (liveColour, deadColour, colourLerpCounterDie);
			if (colourLerpCounterDie < 1) 
			{ 
				colourLerpCounterDie += Time.deltaTime / colourLerpDurationDie;
			}
		}
	}

	public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
		hp -= damage;

		if (materialHolderBody != null) 
		{
			materialHolderBody.GetComponent<Renderer> ().material.color = hitColour;
		}
		if (materialHolderHead != null) 
		{
			materialHolderHead.GetComponent<Renderer> ().material.color = hitColour;
		}
		Invoke ("RecoverColour", 0.1f);

		if (hp <= 0 && !isDead) 
		{
			Die ();
		} 

		RpcHitPlaceEffect (hitPoint, hitDirection);
		HitPlaceEffect (hitPoint, hitDirection);

	}

	public void RpcHitPlaceEffect(Vector3 hitPoint, Vector3 hitDirection)
	{
		HitPlaceEffect (hitPoint, hitDirection);
	}

	public virtual void HitPlaceEffect(Vector3 hitPoint, Vector3 hitDirection)
	{
		////// МАГИЯ 
		/// 
		GameObject newHitEffect = Instantiate (hitEffect, hitPoint, Quaternion.FromToRotation (Vector3.forward, hitDirection));
		Destroy (newHitEffect, 3f);
		newHitEffect.GetComponent<ParticleSystemRenderer> ().material = bludMaterial;

		//NetworkServer.Spawn (newHitEffect);
	}


	public virtual void Die()
	{

		isDead = true;
		//Destroy (gameObject);
	}

	void RecoverColour()
	{
		if (materialHolderBody != null) 
		{
			materialHolderBody.GetComponent<Renderer> ().material.color = liveColour;
		}
		if (materialHolderHead != null) 
		{
			materialHolderHead.GetComponent<Renderer> ().material.color = liveColour;
		}
	}
}
