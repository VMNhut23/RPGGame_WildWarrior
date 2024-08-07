using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2;
    private Player player;

	private bool canRotate = true;
	private bool isReturning;

	private float freezeTimerDuration;
	private float returnSpeed = 12;

	[Header("Perice info")]
	[SerializeField] private float periceAmount;

	[Header("Bounce info")]
	private float bounceSpeed;
	private bool isBouncing;
	private int bounceAmount = 4;
	private List<Transform> enemyTarget;
	private int targetIndex;

	[Header("Spin info")]
	private float maxTravelDistance;
	private float spinDuration;
	private float spinTimer;
	private bool wasStopped;
	private bool isSpinning;

	private float hitTimer;
	private float hitCooldown;

	private void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
		circleCollider2 = GetComponent<CircleCollider2D>();
	}
	public void DestroyMe()
	{
		Destroy(gameObject);
	}
	public void SetUpSword(Vector2 _dir, float gravityScale, Player _player, float _freezeTimerDuration, float _returnSpeed)
	{
		player = _player;
		freezeTimerDuration = _freezeTimerDuration;
		returnSpeed = _returnSpeed;

		rb.velocity = _dir;
		rb.gravityScale = gravityScale;
		if(periceAmount <= 0)
			anim.SetBool("Rotation", true);

		Invoke("DestroyMe", 7);
	}
	public void SetupBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
	{
		isBouncing = _isBouncing;
		bounceAmount = _amountOfBounce;
		bounceSpeed = _bounceSpeed;
		enemyTarget = new List<Transform>();
	}
	public void SetupPerice(int _periceAmount)
	{
		periceAmount = _periceAmount;
	}
	public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
	{
		isSpinning = _isSpinning;
		maxTravelDistance = _maxTravelDistance;
		spinDuration = _spinDuration;
		hitCooldown = _hitCooldown;
	}
	public void ReturnSword()
	{
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		//rb.isKinematic = false;
		transform.parent = null;
		isReturning = true;
	}
	private void Update()
	{
		if (canRotate)
			transform.right = rb.velocity;

		if (isReturning)
		{
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

			if (Vector2.Distance(transform.position, player.transform.position) < 1)
				player.CatchTheSword();
		}

		BounceLogic();
		SpinLogic();
	}

	private void SpinLogic()
	{
		if (isSpinning)
		{
			if (Vector2.Distance(player.transform.position, transform.position) > maxTravelDistance && !wasStopped)
			{
				StopWhenSpinning();
			}
			if (wasStopped)
			{
				spinTimer -= Time.deltaTime;
				if (spinTimer < 0)
				{
					isReturning = true;
					isSpinning = false;
				}

				hitTimer -= Time.deltaTime;
				if (hitTimer < 0)
				{
					hitTimer = hitCooldown;

					Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, 1);
					foreach (var hit in collider2D)
					{
						if (hit.GetComponent<Enemy>() != null)
							SwordSkillDamage(hit.GetComponent<Enemy>());
					}
				}
			}
		}
	}

	private void StopWhenSpinning()
	{
		wasStopped = true;
		rb.constraints = RigidbodyConstraints2D.FreezePosition;
		spinTimer = spinDuration;
	}

	private void BounceLogic()
	{
		if (isBouncing && enemyTarget.Count > 0)
		{
			transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
			if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
			{
				SwordSkillDamage(enemyTarget[targetIndex].GetComponent<Enemy>());

				targetIndex++;
				bounceAmount--;

				if (bounceAmount <= 0)
				{
					isBouncing = false;
					isReturning = true;
				}
				if (targetIndex >= enemyTarget.Count)
					targetIndex = 0;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (isReturning)
			return;

		if(collision.GetComponent<Enemy>() != null)
		{
			Enemy enemy = collision.GetComponent<Enemy>();
			SwordSkillDamage(enemy);

		}
		SetupTargetsForBounce(collision);
		StuckInfo(collision);
	}

	private void SwordSkillDamage(Enemy enemy)
	{
		EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
		player.stats.DoDamage(enemyStats);

		if(player.skill.sword.timeStopUnlocked)
			enemy.FreezeTimeFor(freezeTimerDuration);
		if (player.skill.sword.vulnerableUnlocked)
			enemyStats.MakeVulnerableFor(freezeTimerDuration);

		ItemData_Equipment equipedAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);

		if (equipedAmulet != null)
			equipedAmulet.Effect(enemy.transform);
	}

	private void SetupTargetsForBounce(Collider2D collision)
	{
		if (collision.GetComponent<Enemy>() != null)
		{
			if (isBouncing && enemyTarget.Count <= 0)
			{
				Collider2D[] collider2D = Physics2D.OverlapCircleAll(transform.position, 10);
				foreach (var hit in collider2D)
				{
					if (hit.GetComponent<Enemy>() != null)
						enemyTarget.Add(hit.transform);
				}
			}
		}
	}

	private void StuckInfo(Collider2D collision)
	{
		if(periceAmount > 0 && collision.GetComponent<Enemy>() != null)
		{
			periceAmount--;
			return;
		}

		if (isSpinning)
		{
			StopWhenSpinning();
			return;
		}

		canRotate = false;
		circleCollider2.enabled = false;

		rb.isKinematic = true;
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		GetComponentInChildren<ParticleSystem>().Play();

		if (isBouncing && enemyTarget.Count > 0)
			return;

		anim.SetBool("Rotation", false);
		transform.parent = collision.transform;
	}
}
