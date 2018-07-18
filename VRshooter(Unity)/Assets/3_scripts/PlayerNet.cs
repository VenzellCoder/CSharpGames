using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNet : Unit {



	GunController gunController;


	public override void Start () 
	{
		base.Start ();


		Cursor.visible = false;

		gunController = GetComponent<GunController> (); 

	}
	

	public override void Update () 
	{
		//base.Update ();


		if (Input.touchCount > 0) {

			gunController.Shoot();
		}

		// Ввод стрельба

		if (Input.GetMouseButton(0))
		{
			gunController.Shoot();
		}
	}
}
