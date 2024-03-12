using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
	[SerializeField] private GameObject hotKeyPrefab;
	[SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
	private float shrinkSpeed;
	private float blackholeTimer;

	private bool canGrow = true;
	private bool canShrink;
	private bool canCreateHotKeys = true;
	private bool cloneAttackReleased;
	private bool playerCanDisapear = true;

	private int amountOfAttacks = 4;
	private float cloneAttackCooldown = .3f;
	private float cloneAttackTimer;

	private List<Transform> targets = new List<Transform>();
	private List<GameObject> createdHotKey = new List<GameObject>();

	public bool playerCanExitState { get; private set; }
	public void SetupBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuration)
	{
		maxSize = _maxSize;
		growSpeed = _growSpeed;
		shrinkSpeed = _shrinkSpeed;
		amountOfAttacks = _amountOfAttacks;
		cloneAttackCooldown = _cloneAttackCooldown;

		blackholeTimer = _blackholeDuration;

		if (SkillManager.instance.clone.crystalInsteadOfClone)
			playerCanDisapear = false;
	}

	private void Update()
	{
		cloneAttackTimer -= Time.deltaTime;
		blackholeTimer -= Time.deltaTime;

		if(blackholeTimer < 0)
		{
			blackholeTimer = Mathf.Infinity;

			if (targets.Count > 0)
				ReleaseCloneAttack();
			else
				FinishBlackHoleAbility();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			ReleaseCloneAttack();
		}

		CloneAttackLogic();

		if (canGrow && !canShrink)
		{
			transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
		}
		if (canShrink)
		{
			transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

			if (transform.localScale.x < 0)
				Destroy(gameObject);
			
		}
	}

	private void ReleaseCloneAttack()
	{
		if (targets.Count <= 0)
			return;

		DestroyHotKey();
		cloneAttackReleased = true;
		canCreateHotKeys = false;

		if (playerCanDisapear)
		{
			playerCanDisapear = false;
			PlayerManager.instance.player.MakeTransprent(true);
		}
	}

	private void CloneAttackLogic()
	{
		if (cloneAttackTimer < 0 && cloneAttackReleased && amountOfAttacks > 0)
		{
			cloneAttackTimer = cloneAttackCooldown;

			int randomIndex = Random.Range(0, targets.Count);

			float xOffset;
			if (Random.Range(0, 100) > 50)
				xOffset = 2;
			else
				xOffset = -2;

			if (SkillManager.instance.clone.crystalInsteadOfClone)
			{
				SkillManager.instance.crystal.CreateCrystal();
				SkillManager.instance.crystal.CurrentCrystalChooseRandomTarget();
			}
			else
			{
				SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
			}
			amountOfAttacks--;
			if (amountOfAttacks <= 0)
			{
				Invoke("FinishBlackHoleAbility", 0.9f);
			}
		}
	}

	private void FinishBlackHoleAbility()
	{
		DestroyHotKey();
		playerCanExitState = true;
		canShrink = true;
		cloneAttackReleased = false;
	}

	private void DestroyHotKey()
	{
		if (createdHotKey.Count <= 0)
			return;
		for (int i = 0; i < createdHotKey.Count; i++)
		{
			Destroy(createdHotKey[i]);
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<Enemy>() != null)
		{
			collision.GetComponent<Enemy>().FreezeTimer(true);

			CreateHotKey(collision);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.GetComponent<Enemy>() != null)
			collision.GetComponent<Enemy>().FreezeTimer(false);
	}

	private void CreateHotKey(Collider2D collision)
	{
		if(keyCodeList.Count <= 0)
		{
			Debug.LogWarning("Not enough hot keys in a key code list");
			return;
		}
		if (!canCreateHotKeys)
			return;

		GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
		createdHotKey.Add(newHotKey);
		KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
		keyCodeList.Remove(choosenKey);

		Blackhole_HotKey_Controller newHotKeyScripts = newHotKey.GetComponent<Blackhole_HotKey_Controller>();
		newHotKeyScripts.SetupHotKey(choosenKey, collision.transform, this);
	}
	public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
