using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveMananger : MonoBehaviour
{
	public static SaveMananger instance;

	[SerializeField] private string fileName;
	[SerializeField] private bool encryptData;

	private GameData gameData;
	private List<ISaveManager> saveManagers;
	private FileDataHandler dataHandler;

	[ContextMenu("Delete saved data")]
	public void DeleteSavedData()
	{
		dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
		dataHandler.Delete();
	}

	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		else
			instance = this;
	}
	private void Start()
	{
		dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
		saveManagers = FindAllSaveManagers();

		LoadGame();
	}
	public void NewGame()
	{
		gameData = new GameData();
	}
	public void LoadGame()
	{
		gameData = dataHandler.Load();

		if(this.gameData == null)
		{
			Debug.Log("No save data found");
			NewGame();
		}
		foreach (ISaveManager saveManager in saveManagers)
		{
			saveManager.LoadData(gameData);
		}
	}
	public void SaveGame()
	{
		foreach (ISaveManager saveManager in saveManagers)
		{
			saveManager.SaveData(ref gameData);
		}
		dataHandler.Save(gameData);
	}
	private void OnApplicationQuit()
	{
		SaveGame();
	}
	private List<ISaveManager> FindAllSaveManagers()
	{
		IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();

		return new List<ISaveManager>(saveManagers);
	}

	public bool HaveSavedData()
	{
		if (dataHandler.Load() != null)
			return true;
		return false;
	}
}
