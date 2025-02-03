using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_SkillTreeTooltip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI skillDescription;
	[SerializeField] private TextMeshProUGUI skillName;
	[SerializeField] private TextMeshProUGUI skillCost;
	[SerializeField] private float defaultNameFontSize;

	public void ShowTooltip(string _skillDescription, string _skillName, int _price)
	{

		skillName.text = _skillName;
		skillDescription.text = _skillDescription;
		skillCost.text = "Cost: " + _price;

		AdjustPosition();

		AdjustFontSize(skillName);

		gameObject.SetActive(true);
	}
	public void HideTooltip()
	{
		skillName.fontSize = defaultNameFontSize;
		gameObject.SetActive(false);
	}
}
