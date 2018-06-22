using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelWave : MonoBehaviour {

	// префаб GUI панели
	public PanelSubWave panelSubWave;
	public int subWavesNumber;
	public int waveIndex;
	SubWave[] wave;


	public void AddPanelSubWave(int _subWaveIndex)
	{
		PanelSubWave newPanel = Instantiate (panelSubWave);
		newPanel.waveIndex = waveIndex;
		newPanel.subWaveIndex = _subWaveIndex;

		newPanel.Initialization (wave[_subWaveIndex]);

		newPanel.transform.SetParent (transform);
		newPanel.transform.SetSiblingIndex (transform.childCount-1);

	}

	public void Initialization(SubWave[] _wave)
	{
		wave = new SubWave[_wave.Length];
		wave = _wave;

		subWavesNumber = wave.Length;
		
		for (int sw = 0; sw < subWavesNumber; sw++) 
		{
			AddPanelSubWave (sw);
		}

	}
}
