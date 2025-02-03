using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour, ISaveManager
{
    [Header("End screen")]
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject menuGame;
    public UI_FadeScreen fadeScreen;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject inGameUI;

    public UI_SkillTreeTooltip skillTooltip;
    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;
    public UI_CraftWindow craftWindow;

    [SerializeField] private UI_VolumeSlider[] volumeSettings;

	private void Awake()
	{
        SwitchTo(skillTreeUI);
        fadeScreen.gameObject.SetActive(true);
	}
	void Start()
    {
        SwitchTo(inGameUI);

        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }
    void Update()
    {
        /*if (Input.GetKeyDown
         * 
         * (KeyCode.C))
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.X))
            SwitchWithKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.V))
            SwitchWithKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(optionUI);*/
        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchWithKeyTo(optionUI);
    }
    public void SwitchTo(GameObject _menu)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;

            if(fadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
		}

        if (_menu != null)
		{
            AudioManager.instance.PlaySFX(7, null);
		}
            _menu.SetActive(true);

        if(GameManager.instance != null)
		{
            if (_menu == inGameUI)
                GameManager.instance.PauseGame(false);
            else
                GameManager.instance.PauseGame(true);
		}
	}
    public void SwitchWithKeyTo(GameObject _menu)
	{
        if(_menu != null && _menu.activeSelf)
		{
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
		}
        SwitchTo(_menu);
	}
    private void CheckForInGameUI()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
		}
        SwitchTo(inGameUI);
	}
    public void SwitchOnEndScreen()
	{
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCoroutine());
	}
    public void SwitchOnWinScreen()
	{
        fadeScreen.FadeOut();
        StartCoroutine(WinScreenCoroutine());

	}
    IEnumerator EndScreenCoroutine()
	{
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        restartButton.SetActive(true);
        menuGame.SetActive(true);
	}
    IEnumerator WinScreenCoroutine()
    {
        yield return new WaitForSeconds(1);
        winText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        menuGame.SetActive(true);
    }
    public void RestartGameButton() => GameManager.instance.RestartScene();
    public void MenuGame() => GameManager.instance.MenuGame();

	public void LoadData(GameData _data)
	{
		foreach(KeyValuePair<string, float> pair in _data.volumeSettings)
		{
			foreach (UI_VolumeSlider item in volumeSettings)
			{
                if (item.paramater == pair.Key)
                    item.LoadSlider(pair.Value);
			}
		}
	}

	public void SaveData(ref GameData _data)
	{
        _data.volumeSettings.Clear();
		foreach (UI_VolumeSlider item in volumeSettings)
		{
            _data.volumeSettings.Add(item.paramater, item.slider.value);
		}
	}
}
