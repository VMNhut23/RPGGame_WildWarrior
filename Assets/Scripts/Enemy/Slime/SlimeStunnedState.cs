using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStunnedState : EnemyState
{
	private Enemy_Slime enemy;
	public SlimeStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Slime _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}
	public override void Enter()
	{
		base.Enter();

		enemy.entityFX.InvokeRepeating("RedColorBlink", 0, .1f);

		stateTimer = enemy.stunDuration;

		enemy.rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.x, enemy.stunDirection.y);
	}

	public override void Exit()
	{
		base.Exit();
		enemy.stats.MakeInvincible(false);
	}

	public override void Update()
	{
		base.Update();

		if(enemy.rb.velocity.y < .1f && enemy.IsGroundedDetected())
		{
			enemy.entityFX.Invoke("CancelColorChange", 0);
			enemy.animator.SetTrigger("StunFold");
			enemy.stats.MakeInvincible(true);
		}

		if (stateTimer < 0)
			stateMachine.ChangeState(enemy.idleState);
	}
}
