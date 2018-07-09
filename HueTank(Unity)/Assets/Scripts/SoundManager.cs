using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	float masterVolume = 1;
	float soundsVolume = 1;
	float musicVolume = 1;

	static public SoundManager instance = null;

	// реализаация паттерна Одиночка 
	void Awake ()
	{
		if (instance != null) 
		{
			Destroy (gameObject);
		} 
		else 
		{
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}

	public void PlaySound(AudioClip clip, Vector3 soundPosition)
	{
		AudioSource.PlayClipAtPoint (clip, soundPosition, soundsVolume * masterVolume);
	}
}
