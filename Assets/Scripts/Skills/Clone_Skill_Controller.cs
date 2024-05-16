using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
	private Player player;
	private SpriteRenderer sr;
	private Animator animator;
	[SerializeField] private float colorLosingSpeed;

	private float cloneTimer;
	private float attackMultiplier;
	[SerializeField] private Transform attackCheck;
	[SerializeField] private float attackCheckRadius = 0.8f;
	private Transform closestEnemy;
	private float facingDir = 1;

	private bool canDuplicateClone;
	private float changeToDuplicate;
	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}
	private void Update()
	{
		cloneTimer -= Time.deltaTime;
		if(cloneTimer < 0)
		{
			sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));

			if (sr.color.a <= 0)
				Destroy(gameObject);
		}
	}
	public void SetupClone(Transform _newTransform, float _cloneDuration, bool canAttack, Vector3 _offset, Transform _closestEnemy, bool _canDuplicateClone, float _changeToDuplicate,Player _player, float _attackMultiplier)
	{
		if (canAttack)
		{
			animator.SetInteger("AttackNumber", Random.Range(1, 3));
		}
		attackMultiplier = _attackMultiplier;
		player = _player;
		transform.position = _newTransform.position + _offset;
		cloneTimer = _cloneDuration;
		closestEnemy = _closestEnemy;
		canDuplicateClone = _canDuplicateClone;
		changeToDuplicate = _changeToDuplicate;
		FaceClosestTarget();
	}
	public void AnimationTrigger()
	{
		cloneTimer = -.1f;
	}
	private void AttackTrigger()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<Enemy>() != null)
			{
				/*player.stats.DoDamage(hit.GetComponent<CharacterStats>());*/

				PlayerStats playerStats = player.GetComponent<PlayerStats>();
				EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

				playerStats.CloneDoDamage(enemyStats, attackMultiplier);

				if (player.skill.clone.canApplyOnHitEffect)
				{
					ItemData_Equipment weaponData = Inventory.instance.GetEquipment(EquipmentType.Weapon);

					if (weaponData != null)
						weaponData.Effect(hit.transform);
				}

				if (canDuplicateClone)
				{
					if(Random.Range(0,100) < changeToDuplicate)
					{
						SkillManager.instance.clone.CreateClone(hit.transform, new Vector3(1.5f * facingDir,0));
					}
				}
			}
		}
	}
	private void FaceClosestTarget()
	{
		if(closestEnemy != null)
		{
			if (transform.position.x > closestEnemy.position.x)
			{
				facingDir = -1;
				transform.Rotate(0, 180, 0);
			}
		}
	}
}
