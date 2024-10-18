using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherIdleState : ArcherGroundedState
{
	public ArcherIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
	{
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
		if (stateTimer < 0)
			stateMachine.ChangeState(enemy.moveState);
	}
}
