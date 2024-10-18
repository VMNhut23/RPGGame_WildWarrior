using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerIdleState : EnemyState
{
	private Enemy_DeathBringer enemy;
	public DeathBringerIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}
	public override void Enter()
	{
		base.Enter();
		stateTimer = enemy.idleTimer;
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		if (Input.GetKeyDown(KeyCode.K))
			stateMachine.ChangeState(enemy.teleportState);
	}
}
