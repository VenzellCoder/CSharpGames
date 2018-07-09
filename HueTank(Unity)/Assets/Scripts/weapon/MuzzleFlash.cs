using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour {

	public float flashTime;

	void Start()
	{
		Deactivate ();
	}

	public void Activate()
	{
		gameObject.SetActive (true);
		Invoke ("Deactivate", flashTime);
	}

	public void Deactivate()
	{
		gameObject.SetActive (false);
	}
}
