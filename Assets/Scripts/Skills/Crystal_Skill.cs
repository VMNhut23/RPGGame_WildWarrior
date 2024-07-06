using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal_Skill : Skill
{
	[SerializeField] private float crystalDuration;
    [SerializeField] private GameObject crystalPrefab;
	private GameObject currentCrystal;

	[Header("Crystal mirage")]
	[SerializeField] private UI_SkillTreeSlot unlockCloneInsteadButton;
	[SerializeField] private bool cloneInsteadOfCrystal;

	[Header("Crystal simple")]
	[SerializeField] private UI_SkillTreeSlot unlockCrystalButton;
	public bool crystalUnlocked { get; private set; }


	[Header("Explosive crystal")]
	[SerializeField] private UI_SkillTreeSlot unlockExplosiveButton;
	[SerializeField] private bool canExplode;

	[Header("Moving crystal")]
	[SerializeField] private UI_SkillTreeSlot unlockMovingCrystalButton;
	[SerializeField] private bool canMoveToEnemy;
	[SerializeField] private float moveSpeed;

	[Header("Multi stacking crystal")]
	[SerializeField] private UI_SkillTreeSlot unlockMultiStackButton;
	[SerializeField] private bool canUseMultiStacks;
	[SerializeField] private int amountOfStack;
	[SerializeField] private float multiStackCooldown;
	[SerializeField] private float useTimeWindow;
	[SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();

	protected override void Start()
	{
		base.Start();

		unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
		unlockCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockCrytalMirage);
		unlockExplosiveButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
		unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
		unlockMultiStackButton.GetComponent<Button>().onClick.AddListener(UnlockMultiStack);
	}
	protected override void CheckUnlock()
	{
		UnlockCrystal();
		UnlockCrytalMirage();
		UnlockMovingCrystal();
		UnlockExplosiveCrystal();
		UnlockMultiStack();
	}
	#region Unlock skill region
	private void UnlockCrystal()
	{
		if (unlockCrystalButton.unlocked)
			crystalUnlocked = true;
	}
	private void UnlockCrytalMirage()
	{
		if (unlockCloneInsteadButton.unlocked)
			cloneInsteadOfCrystal = true;
	}
	private void UnlockExplosiveCrystal()
	{
		if (unlockExplosiveButton.unlocked)
			canExplode = true;
	}
	private void UnlockMovingCrystal()
	{
		if (unlockMovingCrystalButton.unlocked)
			canMoveToEnemy = true;
	}
	private void UnlockMultiStack()
	{
		if (unlockMultiStackButton.unlocked)
			canUseMultiStacks = true;
	}
	#endregion
	public override void UseSkill()
	{
		base.UseSkill();

		if (CanUseMultiCrystal())
			return;

		if (currentCrystal == null)
		{
			CreateCrystal();
		}
		else
		{
			if (canMoveToEnemy)
				return;

			Vector2 playerPos = player.transform.position;
			player.transform.position = currentCrystal.transform.position;
			currentCrystal.transform.position = playerPos;

			if (cloneInsteadOfCrystal)
			{
				SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
				Destroy(currentCrystal);
			}
			else
			{
				currentCrystal.GetComponent<Crystal_Skill_Controller>()?.FinishCrystal();
			}

		}
	}

	public void CreateCrystal()
	{
		currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
		Crystal_Skill_Controller currentCrystalScripts = currentCrystal.GetComponent<Crystal_Skill_Controller>();

		currentCrystalScripts.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform),player);
	}
	public void CurrentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();
	private bool CanUseMultiCrystal()
	{
		if (canUseMultiStacks)
		{
			if(crystalLeft.Count > 0)
			{
				if (crystalLeft.Count == amountOfStack)
					Invoke("ResetAbility", useTimeWindow);
				cooldown = 0;

				GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
				GameObject newCrystal = Instantiate(crystalToSpawn,player.transform.position, Quaternion.identity);

				crystalLeft.Remove(crystalToSpawn);

				newCrystal.GetComponent<Crystal_Skill_Controller>().
					SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform),player);

				if(crystalLeft.Count <= 0)
				{
					cooldown = multiStackCooldown;
					RefilCrystal();
				}
				return true;
			}	
		}
		return false;
	}
	private void RefilCrystal()
	{
		int amountToAdd = amountOfStack - crystalLeft.Count;
		for (int i = 0; i < amountToAdd; i++)
		{
			crystalLeft.Add(crystalPrefab);
		}
	}
	private void ResetAbility()
	{
		if (cooldownTimer > 0)
			return;
		cooldownTimer = multiStackCooldown;
		RefilCrystal();
	}
}
