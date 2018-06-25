using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour {
	/*
	public Transform pelvis;
	public Transform thighL;
	public Transform calfL;
	public Transform thighR;
	public Transform calfR;
	public Transform spine;
	public Transform upperArmL;
	public Transform foreArmL;
	public Transform head;
	public Transform upperArmR;
	public Transform foreArmR;
*/
	public Transform[] rgParts;



	
	// Update is called once per frame
	public void TurnOnRagDall () 
	{
		foreach (Transform transform in rgParts) 
		{
			transform.GetComponent<Rigidbody> ().isKinematic = false;

			if (transform.GetComponent<CapsuleCollider> () != null) 
			{
				transform.GetComponent<CapsuleCollider> ().enabled = true;
			}
			if (transform.GetComponent<BoxCollider> () != null) 
			{
				transform.GetComponent<BoxCollider> ().enabled = true;
			}
		}
	}

	public IEnumerator TurnOffRagDoll()
	{
		yield return new WaitForSeconds(2f);

		foreach (Transform transform in rgParts) 
		{
			transform.GetComponent<Rigidbody> ().isKinematic = true;

			if (transform.GetComponent<CapsuleCollider> () != null) 
			{
				transform.GetComponent<CapsuleCollider> ().enabled = false;
			}
			if (transform.GetComponent<BoxCollider> () != null) 
			{
				transform.GetComponent<BoxCollider> ().enabled = false;
			}
		}
	}

	public void HeadDown()
	{
		Vector3 fallVector = new Vector3 (Random.Range (-1f, 1f), 0f, Random.Range (-1f, 1f));
		rgParts[8].GetComponent<Rigidbody> ().AddForce (fallVector.normalized*Random.Range (500f, 2000f));


	}
}
