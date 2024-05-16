using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clone_Skill : Skill
{
	[Header("Clone info")]
	[SerializeField] private float attackMultiplier;
    [SerializeField] private GameObject clonePrefabs;
	[SerializeField] private float cloneDuration;
	[Space]

	[Header("Clone attack")]
	[SerializeField] private UI_SkillTreeSlot clonkAttackUnlockButton;
	[SerializeField] private float cloneAttackMultiplier;
	[SerializeField] private bool canAttack;

	[Header("Aggresive clone")]
	[SerializeField] private UI_SkillTreeSlot aggresiveCLoneUnlockButton;
	[SerializeField] private float aggresiveCloneAttackMultiplier;
	public bool canApplyOnHitEffect;

	[Header("Multiple clone")]
	[SerializeField] private UI_SkillTreeSlot multipleUnlockButton;
	[SerializeField] private float multiCloneAttackMultiplier;
	[SerializeField] private bool canDuplicateClone;
	[SerializeField] private float changeToDuplicate;

	[Header("Crystal instead of clone")]
	[SerializeField] private UI_SkillTreeSlot crystalInsteadUnlockButton;
	public bool crystalInsteadOfClone;

	protected override void Start()
	{
		base.Start();

		clonkAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
		aggresiveCLoneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggresiveClone);
		multipleUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
		crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInstead);
	}
	#region Unlock region
	private void UnlockCloneAttack()
	{
		if (clonkAttackUnlockButton.unlocked)
		{
			canAttack = true;
			attackMultiplier = cloneAttackMultiplier;
		}
	}
	private void UnlockAggresiveClone()
	{
		if (aggresiveCLoneUnlockButton.unlocked)
		{
			canApplyOnHitEffect = true;
			attackMultiplier = aggresiveCloneAttackMultiplier;
		}
	}
	private void UnlockMultiClone()
	{
		if (multipleUnlockButton.unlocked)
		{
			canDuplicateClone = true;
			attackMultiplier = multiCloneAttackMultiplier;
		}
	}
	private void UnlockCrystalInstead()
	{
		if (crystalInsteadUnlockButton.unlocked)
			crystalInsteadOfClone = true;
	}
	#endregion
	public void CreateClone(Transform _cloneTransform, Vector3 _offset)
	{
		if (crystalInsteadOfClone)
		{
			SkillManager.instance.crystal.CreateCrystal();
			return;
		}

		GameObject newClone = Instantiate(clonePrefabs);
		newClone.GetComponent<Clone_Skill_Controller>().
			SetupClone(_cloneTransform, cloneDuration, canAttack, _offset, FindClosestEnemy(newClone.transform), canDuplicateClone, changeToDuplicate,player,attackMultiplier);
	}
	public void CreateCloneWithDelay(Transform _enemyTranform)
	{
		StartCoroutine(CloneCreateCoroutine(_enemyTranform, new Vector3(2 * player.facingDir, 0)));
	}
	private IEnumerator CloneCreateCoroutine(Transform _tranform, Vector3 _offset)
	{
		yield return new WaitForSeconds(0.4f);
		CreateClone(_tranform, _offset);
	}
}
