using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelMain : MonoBehaviour {
	
	// префаб панели
	public PanelWave panelWave;
	public PanelWave[] wavesPanelsArray;
	// массив данных о волнах
	public SubWave[,] wavesArray;

	public Slider wavesAmountSliderUI;
	public Slider subWavesAmountSliderUI;
	public Text wavesAmountText;
	public Text subWavesAmountText;

	private int wavesAmount;
	private int subWavesAmount;

	void Start()
	{
		wavesArray = new SubWave[10, 10];
	}

	// инициализация панели волны
	public void AddPanelWave(int _waveIndex)
	{
		PanelWave newPanel = Instantiate (panelWave);
		newPanel.waveIndex = _waveIndex;

		SubWave[] wave = new SubWave[subWavesAmount];
		for (int i = 0; i < subWavesAmount; i++) 
		{
			wave [i] = wavesArray [_waveIndex, i];
		}

		newPanel.Initialization (wave);

		newPanel.transform.SetParent (transform);
		newPanel.transform.SetSiblingIndex (transform.childCount-1);
	}

	// удалить все панели GUI
	public void Clear()
	{
		if (transform.childCount > 0) 
		{
			for (int i = 0; i < transform.childCount; i++) {
				Destroy (transform.GetChild (i).gameObject);
			}
		}
	}


	public void Initialization()
	{
		// обновление массива с данными о волнах
		wavesArray = DataTransfer.levelsWavesList[DataTransfer.chosenLevel];

		// кол-во волн и этапов определяются слайдерами
		wavesAmount = (int)wavesAmountSliderUI.value;
		subWavesAmount = (int)subWavesAmountSliderUI.value;

		wavesAmountText.text = "Волн: " + wavesAmount;
		subWavesAmountText.text = "Этапы в волнах: " + subWavesAmount;

		DataTransfer.wavesAmount = wavesAmount;
		DataTransfer.subWavesAmount = subWavesAmount;

		// создание панелей - волн
		for (int w = 0; w < wavesAmount; w++) 
		{
			AddPanelWave (w);
		}
	}

	// изменение GUI при движении слайдеров
	public void SlidersUpdate()
	{
		Clear ();
		Initialization ();
	}
}
