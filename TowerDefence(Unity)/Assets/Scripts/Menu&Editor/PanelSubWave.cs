using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSubWave : MonoBehaviour {

	public int subWaveIndex;
	public int waveIndex;

	public string enemyType;
	public int enemyAmount;
	public float delayBetween;
	public float delayEnd;

	public Text labelText;
	public Dropdown enemyTypeField;
	public InputField enemyAmountField;
	public InputField delayBetweenEnemiesField;
	public InputField delayEndField;

	private SubWave subWaveInfo;


	public void TypeUpdate()
	{
		if (enemyTypeField.value == 0) enemyType = "A";
		else if (enemyTypeField.value == 1) enemyType = "B";
		else if (enemyTypeField.value == 2) enemyType = "C";

		DataTransfer.levelsWavesList [DataTransfer.chosenLevel][waveIndex, subWaveIndex].type = enemyType;


	}

	public void AmountUpdate()
	{

		enemyAmount = int.Parse(enemyAmountField.text); 
		DataTransfer.levelsWavesList [DataTransfer.chosenLevel][waveIndex, subWaveIndex].amount = enemyAmount;
	}

	public void DelayBetweenUpdate()
	{
		delayBetween = float.Parse(delayBetweenEnemiesField.text); 
		DataTransfer.levelsWavesList [DataTransfer.chosenLevel][waveIndex, subWaveIndex].delayBetween = delayBetween;

	}

	public void DelayEndUpdate()
	{
		delayEnd = float.Parse(delayEndField.text); 
		DataTransfer.levelsWavesList [DataTransfer.chosenLevel][waveIndex, subWaveIndex].delayEnd = delayEnd;
	}


	public void PanelDestroy()
	{
		Destroy (gameObject);
	}


	public void Initialization(SubWave _subWaveInfo)
	{
		labelText.text = "волна " + (waveIndex+1) + ", этап " + (subWaveIndex+1);

		subWaveInfo = _subWaveInfo;

		if (subWaveInfo.type == "A") enemyTypeField.value = 0;
		else if (subWaveInfo.type == "B") enemyTypeField.value = 1;
		else if (subWaveInfo.type == "C") enemyTypeField.value = 2;
		enemyAmountField.text = subWaveInfo.amount.ToString();
		delayBetweenEnemiesField.text = subWaveInfo.delayBetween.ToString();
		delayEndField.text = subWaveInfo.delayEnd.ToString();
	}


	

}
