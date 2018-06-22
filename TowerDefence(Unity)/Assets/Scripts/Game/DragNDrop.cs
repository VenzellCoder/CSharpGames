using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragNDrop : MonoBehaviour {

	// особенности кнопки покупки башни
	public Tower tower;
	public Text textPrice;
	public GameObject dragableImage;

	private bool accessible = true;
	private bool isDraging = false;

	private PlaySessionManager playSessionManager;

	void Start()
	{

		playSessionManager = GameObject.Find ("PlaySessionManager").GetComponent<PlaySessionManager> ();
			
		textPrice.text = tower.priceBuy.ToString();
	
		// при изменении 
		playSessionManager.OnMoneyChange += UpdateAccessibility;
	}
		
	public void DragStart()
	{
		if (accessible) 
		{
			isDraging = true;
		}
	}
		
	public void DragFinish()
	{
		if (isDraging) 
		{
			isDraging = false;
			dragableImage.transform.position = transform.position;
			Vector3 cursorePlace = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			cursorePlace.z = -2f;
			Tower newUnit = Instantiate (tower, cursorePlace, Quaternion.identity);

			// вычесть деньги
			playSessionManager.ChangeMoney (-tower.priceBuy);
			// статистика 
			playSessionManager.statBuild++;
		}

	}

	// перетаскивание башни
	void Update()
	{
		if (isDraging) 
		{
			dragableImage.transform.position = Input.mousePosition;
		}
	}

	// если денег не хватает - башня недоступна 
	void UpdateAccessibility()
	{
		if (tower.priceBuy > playSessionManager.money) 
		{
			dragableImage.GetComponent<Image> ().color = Color.red;
			accessible = false;

		} 
		else 
		{
			dragableImage.GetComponent<Image> ().color = Color.black;
			accessible = true;
		}
	}
}
