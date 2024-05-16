using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private UI ui;
    private Image skillImage;

	[SerializeField] private int skillPrice;
	[SerializeField] private string skillName;
	[TextArea]
	[SerializeField] private string skillDescription;
	[SerializeField] private Color lockedSkillColor;


    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnlocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;


	private void OnValidate()
	{
		gameObject.name = "SkillTreeSlot_UI - " + skillName;
	}

	private void Start()
	{
		skillImage = GetComponent<Image>();

		ui = GetComponentInParent<UI>();

		skillImage.color = lockedSkillColor;

		GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
	}
	public void UnlockSkillSlot()
	{
		if (PlayerManager.instance.HaveEnoughMoney(skillPrice) == false)
			return;

		for (int i = 0; i < shouldBeUnlocked.Length; i++)
		{
			if(shouldBeUnlocked[i].unlocked == false)
			{
				Debug.Log("Cannot unlock skill");
				return;
			}
		}

		for (int i = 0; i < shouldBeLocked.Length; i++)
		{
			if(shouldBeLocked[i].unlocked == true)
			{
				Debug.Log("Cannot unlock skill");
				return;
			}
		}
		unlocked = true;
		skillImage.color = Color.white;

	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		ui.skillTooltip.ShowTooltip(skillDescription, skillName);
		Vector2 mousePosition = Input.mousePosition;

		float xOffset = 0;
		float yOffset = 0;

		if (mousePosition.x > 600)
			xOffset = -150;
		else
			xOffset = 150;

		if (mousePosition.y > 600)
			yOffset = -150;
		else
			yOffset = 150;

		ui.skillTooltip.transform.position = new Vector2(mousePosition.x + xOffset, mousePosition.y + yOffset);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		ui.skillTooltip.HideTooltip();
	}
}
