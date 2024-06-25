using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parry_Skill : Skill
{
	[Header("Parry")]
	[SerializeField] private UI_SkillTreeSlot parryUnlockButton;
	public bool parryUnlocked;

	[Header("Parry restore")]
	[SerializeField] private UI_SkillTreeSlot restoreUnlockButton;
	[Range(0f, 1f)]
	[SerializeField] private float restoreHealthPercentage;
	public bool restoreUnlocked;

	[Header("Parry with mirage")]
	[SerializeField] private UI_SkillTreeSlot parryWithMirageUnlockButton;
	public bool parryWithMirageUnlocked;

	public override void UseSkill()
	{
		base.UseSkill();

		if (restoreUnlocked)
		{
			int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPercentage);
			player.stats.IncreaseHealthBy(restoreAmount);
		}
	}
	protected override void Start()
	{
		base.Start();

		parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
		restoreUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
		parryWithMirageUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryWithMirage);
	}
	protected override void CheckUnlock()
	{
		base.CheckUnlock();

		UnlockParry();
		UnlockParryRestore();
		UnlockParryWithMirage();
	}
	private void UnlockParry()
	{
		if (parryUnlockButton.unlocked)
			parryUnlocked = true;
	}
	private void UnlockParryRestore()
	{
		if (restoreUnlockButton.unlocked)
			restoreUnlocked = true;
	}
	private void UnlockParryWithMirage()
	{
		if (parryWithMirageUnlockButton.unlocked)
			parryWithMirageUnlocked = true;
	}
	public void MakeMirageOnParry(Transform _respawnTranform)
	{
		if (parryWithMirageUnlocked)
			SkillManager.instance.clone.CreateCloneWithDelay(_respawnTranform);
	}
}