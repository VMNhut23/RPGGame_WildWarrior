using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherJumpState : EnemyState
{
	private Enemy_Archer enemy;
	public ArcherJumpState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void Enter()
	{
		base.Enter();

		enemy.rb.velocity = new Vector2(enemy.jumpVelocity.x * -enemy.facingDir, enemy.jumpVelocity.y);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();

		enemy.animator.SetFloat("yVelocity", enemy.rb.velocity.y);

		if (enemy.rb.velocity.y < 0 && enemy.IsGroundedDetected())
			stateMachine.ChangeState(enemy.battleState);
	}
}
