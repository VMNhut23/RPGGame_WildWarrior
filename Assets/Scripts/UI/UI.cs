using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillTreeUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionUI;
    [SerializeField] private GameObject inGameUI;

    public UI_SkillTreeTooltip skillTooltip;
    public UI_ItemTooltip itemTooltip;
    public UI_StatTooltip statTooltip;
    public UI_CraftWindow craftWindow;
	private void Awake()
	{
        SwitchTo(skillTreeUI);
	}
	void Start()
    {
        SwitchTo(inGameUI);

        itemTooltip.gameObject.SetActive(false);
        statTooltip.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.X))
            SwitchWithKeyTo(skillTreeUI);

        if (Input.GetKeyDown(KeyCode.V))
            SwitchWithKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(optionUI);
    }
    public void SwitchTo(GameObject _menu)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
            transform.GetChild(i).gameObject.SetActive(false);
		}

        if (_menu != null)
            _menu.SetActive(true);
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
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
		}
        SwitchTo(inGameUI);
	}
}
