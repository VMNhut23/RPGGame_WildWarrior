using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
	[SerializeField] private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2;
    private Player player;

	private bool canRotate = true;
	private bool isReturning;

	[Header("Perice info")]
	[SerializeField] private float periceAmount;

	[Header("Bounce info")]
	[SerializeField] private float bounceSpeed;
	private bool isBouncing;
	private int bounceAmount = 4;
	private List<Transform> enemyTarget;
	private int targetIndex;
	private void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
		circleCollider2 = GetComponent<CircleCollider2D>();
	}
	public void SetUpSword(Vector2 _dir, float gravityScale, Player _player)
	{
		player = _player;
		rb.velocity = _dir;
		rb.gravityScale = gravityScale;

		anim.SetBool("Rotation", true);
	}
	public void SetupBounce(bool _isBouncing, int _amountOfBounce)
	{
		isBouncing = _isBouncing;
		bounceAmount = _amountOfBounce;
		enemyTarget = new List<Transform>();
	}
	public void SetupPerice(int _periceAmount)
	{
		periceAmount = _periceAmount;
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
	}
	private void BounceLogic()
	{
		if (isBouncing && enemyTarget.Count > 0)
		{
			transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
			if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
			{
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

		StuckInfo(collision);

	}

	private void StuckInfo(Collider2D collision)
	{
		if(periceAmount > 0 && collision.GetComponent<Enemy>() != null)
		{
			periceAmount--;
			return;
		}

		canRotate = false;
		circleCollider2.enabled = false;

		rb.isKinematic = true;
		rb.constraints = RigidbodyConstraints2D.FreezeAll;

		if (isBouncing && enemyTarget.Count > 0)
			return;

		anim.SetBool("Rotation", false);
		transform.parent = collision.transform;
	}
}
