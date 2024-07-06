using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockDodgeButton;
	[SerializeField] private int evasionAmount;
    public bool dodgeUnlocked;

    [Header("Mirage dodge")]
    [SerializeField] private UI_SkillTreeSlot unlockMirageDodge;
    public bool mirageDodgeUnlocked;
	protected override void Start()
	{
		base.Start();

		unlockDodgeButton.GetComponent<Button>().onClick.AddListener(UnlockDodge);
		unlockMirageDodge.GetComponent<Button>().onClick.AddListener(UnloccMirageDodge);
	}
	protected override void CheckUnlock()
	{
		UnlockDodge();
		UnloccMirageDodge();
	}
	private void UnlockDodge()
	{
		if (unlockDodgeButton.unlocked)
			dodgeUnlocked = true;
	}
	private void UnloccMirageDodge()
	{
		if (unlockMirageDodge.unlocked)
		{
			player.stats.evasion.AddModifier(evasionAmount);
			Inventory.instance.UpdateStatsUI();
			mirageDodgeUnlocked = true;
		}
	}
	public void CreateMirageOnDodge()
	{
		if (mirageDodgeUnlocked)
			SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir,0));
	}
}
