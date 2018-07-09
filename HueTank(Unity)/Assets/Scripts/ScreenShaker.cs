using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShaker : MonoBehaviour {

	public IEnumerator Shake(float duration, float magnitude)
	{
		Vector3 originalPosition = transform.localPosition;

		float counter = 0f;

		while (counter < duration) 
		{
			// затухание колебаний
			float decreaseShakeIndex = 1 - counter / duration;

			float x = Random.Range (-1f, 1f) * magnitude * decreaseShakeIndex;
			float y = Random.Range (-1f, 1f) * magnitude * decreaseShakeIndex;

			transform.localPosition = new Vector3 (x, y, originalPosition.z);

			counter += Time.deltaTime;

			yield return null;
		}
		// возвращение на позицию камеры до скриншейка
		transform.localPosition = originalPosition;
	}

}
