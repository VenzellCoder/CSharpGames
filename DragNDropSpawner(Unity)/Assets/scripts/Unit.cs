using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour {


	public float speed;

	public int number;
	public Text text;
	// канва для цифр
	public Canvas textCanvas;

	void Start () 
	{
		// случайная скорость
		speed = Random.Range (0.5f, 3f);
		// родитель-холст
		text.GetComponent<RectTransform> ().parent = textCanvas.GetComponent<RectTransform> ();
	}
	
	void Update()
	{
		Vector3 textPlace = Camera.main.WorldToScreenPoint (transform.position);
		text.transform.position = new Vector3 (textPlace.x, textPlace.y+60f, textPlace.z);
	}

	void FixedUpdate () 
	{
		// движение
		Vector3 movement = transform.forward * speed * Time.deltaTime;
		transform.position = (transform.position + movement);

		// удаление объекта при достижении конца поля
	}

	void OnCollisionEnter (Collision col)
	{
		if(col.gameObject.name == "Destroyer")
		{
			Destroy (text.gameObject);
			Destroy(gameObject);
		}
	}

}
