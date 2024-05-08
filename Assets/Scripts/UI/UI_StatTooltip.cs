using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_StatTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI description;
    public void ShowStatTooltip(string _text)
	{
		description.text = _text;

		gameObject.SetActive(true);
	}
	public void HideStatTooltip()
	{
		description.text = "";
		gameObject.SetActive(false);
	}
}
