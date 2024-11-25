using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int currency;
	public SerializableDictionary<string, int> inventory;
	public List<string> equipmentId;
	public SerializableDictionary<string, bool> skillTree;
	public SerializableDictionary<string, bool> checkpoint;
	public string closestCheckpointId;

	public float lostCurrencyX;
	public float lostCurrencyY;
	public int lostCurrencyAmount;

	public SerializableDictionary<string, float> volumeSettings;
    public GameData()
	{
		this.lostCurrencyX = 0;
		this.lostCurrencyY = 0;
		this.lostCurrencyAmount = 0;

		this.currency = 0;
		inventory = new SerializableDictionary<string, int>();
		skillTree = new SerializableDictionary<string, bool>();
		equipmentId = new List<string>();
		closestCheckpointId = string.Empty;

		checkpoint = new SerializableDictionary<string, bool>();
		volumeSettings = new SerializableDictionary<string, float>();
	}
}
