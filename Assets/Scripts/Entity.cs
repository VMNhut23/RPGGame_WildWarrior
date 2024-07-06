using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
	[Header("Collision info")]
	public Transform attackCheck;
	public float attackCheckRadius;
	[SerializeField] protected Transform groundCheck;
	[SerializeField] protected float groundCheckDistance;
	[SerializeField] protected Transform wallCheck;
	[SerializeField] protected float wallCheckDistance;
	[SerializeField] protected LayerMask whatIsGround;

	[Header("Knockback info")]
	[SerializeField] protected Vector2 knockbackPower;
	[SerializeField] protected float knockbackDuration;
	protected bool isKnocked;

	public int knockbackDir { get; private set; }
	public int facingDir { get; private set; } = 1;
	protected bool facingRight = true;
	#region Components
	public Animator animator { get; private set; }
	public Rigidbody2D rb { get; private set; }
	public EntityFX entityFX { get; private set; }
	public SpriteRenderer sr { get; private set; }
	public CharacterStats stats { get; private set; }
	public CapsuleCollider2D cd { get; private set; }
	#endregion
	public System.Action onFlipped;
	protected virtual void Awake()
	{
		
	}
	protected virtual void Start()
	{
		sr = GetComponentInChildren<SpriteRenderer>();
		animator = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
		entityFX = GetComponent<EntityFX>();
		stats = GetComponent<CharacterStats>();
		cd = GetComponent<CapsuleCollider2D>();
	}
	protected virtual void Update()
	{
		
	}
	public virtual void SlowEntityBy(float _slowPercentage, float slowDuration)
	{

	}
	protected virtual void ReturnDefaultSpeed()
	{
		animator.speed = 1;
	}
	public virtual void DamageEffect() => StartCoroutine("HitKnockback");
	public virtual void SetupKnockbackDir(Transform _damageDirection)
	{
		if (_damageDirection.position.x > transform.position.x)
			knockbackDir = -1;
		else if (_damageDirection.position.x < transform.position.x)
			knockbackDir = 1;
	}
	public void SetupKnockbackPower(Vector2 _knockbackPower) => knockbackPower = _knockbackPower;
	protected virtual IEnumerator HitKnockback()
	{
		isKnocked = true;
		rb.velocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
		yield return new WaitForSeconds(knockbackDuration);
		isKnocked = false;
		SetupZeroKnockbackPower();
	}
	protected virtual void SetupZeroKnockbackPower()
	{

	}
	#region Velocity
	public void ZeroVelocity()
	{
		if (isKnocked)
			return;
		rb.velocity = new Vector2(0, 0);
	}
	public void SetVelocity(float xVelocity, float yVelocity)
	{
		if (isKnocked)
			return;
		rb.velocity = new Vector2(xVelocity, yVelocity);
		FlipController(xVelocity);
	}
	#endregion

	#region Collision
	public virtual bool IsGroundedDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
	public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
	protected virtual void OnDrawGizmos()
	{
		Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
		Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
		Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
	}
	#endregion
	#region Flip
	public virtual void Flip()
	{
		facingDir = facingDir * -1;
		facingRight = !facingRight;
		transform.Rotate(0, 180, 0);

		if(onFlipped != null)
			onFlipped();
	}
	public virtual void FlipController(float _x)
	{
		if (_x > 0 && !facingRight)
		{
			Flip();
		}
		else if (_x < 0 && facingRight)
		{
			Flip();
		}
	}
	#endregion
	public void MakeTransprent(bool _transprent)
	{
		if (_transprent)
			sr.color = Color.clear;
		else
			sr.color = Color.white;
	}
	public virtual void Die()
	{

	}
}
