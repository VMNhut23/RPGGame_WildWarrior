using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeathBringer : Enemy
{
	#region States
	public DeathBringerIdleState idleState { get; private set; }
	public DeathBringerAttackState attackState { get; private set; }
	public DeathBringerBattleState battleState { get; private set; }
	public DeathBringerDeadState deadState { get; private set; }
	public DeathBringerSpellCastState spellCastState { get; private set; }
	public DeathBringerTeleportState teleportState { get; private set; }
	#endregion

	[Header("Teleport details")]
	[SerializeField] private BoxCollider2D arena;
	[SerializeField] private Vector2 surroundingCheckSize;


	protected override void Awake()
	{
		base.Awake();
		idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
		battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
		attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);

		deadState = new DeathBringerDeadState(this, stateMachine, "Idle", this);

		spellCastState = new DeathBringerSpellCastState(this, stateMachine, "SpellCast", this);
		teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
	}
	protected override void Start()
	{
		base.Start();

		stateMachine.Initialize(idleState);
	}
	public override void Die()
	{
		base.Die();
		stateMachine.ChangeState(deadState);
	}

	public void FindPosition()
	{
		float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
		float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);

		transform.position = new Vector3(x, y);
		transform.position = new Vector3(transform.position.x, transform.position.y - GroundBelow().distance + (cd.size.y / 2));

		if(!GroundBelow() || SomethingIsAround())
		{
			Debug.Log("Looking for new position");
			FindPosition();
		}
	}

	private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, whatIsGround);
	private bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, whatIsGround);
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();

		Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
		Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
	}
}
