using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Path : MonoBehaviour {

	public static Transform[] points;
	int pointsAmount;

	// сохранить все точки пути в массив
	void Awake()
	{
		pointsAmount = transform.childCount;
		points = new Transform[pointsAmount];

		for (int i = 0; i < pointsAmount; i++) 
		{
			points [i] = transform.GetChild (i);
		}
	}
}