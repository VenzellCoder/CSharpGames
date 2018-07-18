using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MuzzleFlash : MonoBehaviour {

	public GameObject flashHolder;
	public float flashTime;
	public Sprite[] flashSprites;
	public SpriteRenderer spriteRenderer;

	void Start()
	{
	Deactivate ();
	}

	public void Activate()
	{
		int flashSpriteIndex = Random.Range (0, flashSprites.Length);

		spriteRenderer.sprite = flashSprites [flashSpriteIndex];


		flashHolder.SetActive (true);
		Invoke ("Deactivate", flashTime);
	}

	public void Deactivate()
	{
		flashHolder.SetActive (false);
	}
}
