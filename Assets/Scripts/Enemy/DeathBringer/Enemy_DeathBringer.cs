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

	protected override void Awake()
	{
		base.Awake();
		idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
		battleState = new DeathBringerBattleState(this, stateMachine, "Battle", this);
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
}
