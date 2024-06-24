using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
	public static GameManager instance;

	[SerializeField] private Checkpoint[] checkpoints;
	public string closestCheckpointId;

	[Header("Lost currency")]
	[SerializeField] private GameObject lostCurrencyPrefab;
	public int lostCurrencyAmount;
	[SerializeField] private float lostCurrencyX;
	[SerializeField] private float lostCurrencyY;
	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		else
			instance = this;
	}
	private void Start()
	{
		checkpoints = FindObjectsOfType<Checkpoint>();
	}
	public void RestartScene()
	{
		SaveMananger.instance.SaveGame();
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

	public void LoadData(GameData _data) => StartCoroutine(LoadWithDelay(_data));

	private void LoadClosestCheckpoint(GameData _data)
	{
		foreach (KeyValuePair<string, bool> pair in _data.checkpoint)
		{
			foreach (Checkpoint checkpoint in checkpoints)
			{
				if (checkpoint.id == pair.Key && pair.Value == true)
					checkpoint.ActiveCheckpoint();
			}
		}
	}

	IEnumerator LoadWithDelay(GameData _data)
	{
		yield return new WaitForSeconds(.1f);

		PlacePlayerAtClosestCheckpoint(_data);
		LoadClosestCheckpoint(_data);
		LoadLostCurrency(_data);
	} 

	private void PlacePlayerAtClosestCheckpoint(GameData _data)
	{
		if (_data.closestCheckpointId == null)
			return;

		closestCheckpointId = _data.closestCheckpointId;

		foreach (Checkpoint checkpoint in checkpoints)
		{
			if (closestCheckpointId == checkpoint.id)
				PlayerManager.instance.player.transform.position = checkpoint.transform.position;
		}
	}

	private void LoadLostCurrency(GameData _data)
	{
		lostCurrencyAmount = _data.lostCurrencyAmount;
		lostCurrencyX = _data.lostCurrencyX;
		lostCurrencyY = _data.lostCurrencyY;

		if(lostCurrencyAmount > 0)
		{
			GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
			newLostCurrency.GetComponent<LostCurrencyController>().currency = lostCurrencyAmount;
		}
		lostCurrencyAmount = 0;
	}

	public void SaveData(ref GameData _data)
	{
		_data.lostCurrencyAmount = lostCurrencyAmount;
		_data.lostCurrencyX = PlayerManager.instance.player.transform.position.x;
		_data.lostCurrencyY = PlayerManager.instance.player.transform.position.y;

		if(FindClosestCheckpoint() != null)
			_data.closestCheckpointId = FindClosestCheckpoint().id;
		_data.checkpoint.Clear();

		foreach (Checkpoint checkpoint in checkpoints)
		{
			_data.checkpoint.Add(checkpoint.id, checkpoint.activationStatus);
		}
	}
	private Checkpoint FindClosestCheckpoint()
	{
		float closestDistance = Mathf.Infinity;
		Checkpoint closestCheckpoint = null;

		foreach (var checkpoint in checkpoints)
		{
			float distanceToCheckpoint = Vector2.Distance(PlayerManager.instance.player.transform.position, checkpoint.transform.position);
			if(distanceToCheckpoint < closestDistance && checkpoint.activationStatus == true)
			{
				closestDistance = distanceToCheckpoint;
				closestCheckpoint = checkpoint;
			}
		}
		return closestCheckpoint;
	}
}
