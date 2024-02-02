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
	[SerializeField] protected Vector2 knockbackDirection;
	[SerializeField] protected float knockbackDuration;
	protected bool isKnocked;

	public int facingDir { get; private set; } = 1;
	protected bool facingRight = true;
	#region Components
	public Animator animator { get; private set; }
	public Rigidbody2D rb { get; private set; }
	public EntityFX entityFX { get; private set; }
	#endregion
	protected virtual void Awake()
	{
		
	}
	protected virtual void Start()
	{
		entityFX = GetComponentInChildren<EntityFX>();
		animator = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}
	protected virtual void Update()
	{
		
	}
	public virtual void Damage()
	{
		entityFX.StartCoroutine("FlashFX");
		StartCoroutine("HitKnockback");
	}
	protected virtual IEnumerator HitKnockback()
	{
		isKnocked = true;
		rb.velocity = new Vector2(knockbackDirection.x * -facingDir, knockbackDirection.y);
		yield return new WaitForSeconds(knockbackDuration);
		isKnocked = false;
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
}
