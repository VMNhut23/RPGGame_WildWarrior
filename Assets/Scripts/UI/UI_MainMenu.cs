using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
	[SerializeField] private GameObject continueButton;
	[SerializeField] private UI_FadeScreen fadeScreen;

	/*private void Start()
	{
		if (SaveMananger.instance.HaveSavedData() == false)
			continueButton.SetActive(false);
	}*/

	public void ContinueGame()
	{
		StartCoroutine(LoadSceneWithFadeEffect(1.5f));
	}
	public void NewGame()
	{
		SaveMananger.instance.DeleteSavedData();
		StartCoroutine(LoadSceneWithFadeEffect(1.5f));
	}
	public void Exit()
	{
		Debug.Log("Exit game");
		//Application.Quit();
	}

	IEnumerator LoadSceneWithFadeEffect(float _delay)
	{
		fadeScreen.FadeOut();

		yield return new WaitForSeconds(_delay);

		SceneManager.LoadScene(sceneName);
	}
}
