using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Все объекты, динамически изменяющие цвет, имеют этот компонент
public class TuneColour : MonoBehaviour {

	// тип объекта
	public enum ColourType{Obstacle, Floor, Enemy, Player};
	public ColourType colourType;

	void Start () 
	{
		// изменять цвета при каждом обновлении палитры в GameManager'е
		GameManager.instance.OnPalleteChange += UpdateColour;

		UpdateColour ();
	}


	public void UpdateColour()
	{
		if (this != null) {

			if (colourType == ColourType.Floor) 
			{
				GetComponent<Renderer> ().material.color = GameManager.instance.floorCol;
			} 
			else if (colourType == ColourType.Obstacle) 
			{
				GetComponent<Renderer> ().material.color = GameManager.instance.obstacleCol;
			} 
			else if (colourType == ColourType.Enemy) 
			{
				GetComponent<Renderer> ().material.color = GameManager.instance.enemyCol;
			} 
			else if (colourType == ColourType.Player) 
			{
				GetComponent<Renderer> ().material.color = GameManager.instance.playerCol;
			}
		}
	}
	
}
