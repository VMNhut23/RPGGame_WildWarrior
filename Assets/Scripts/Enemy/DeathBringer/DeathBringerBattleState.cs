using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBringerBattleState : EnemyState
{
	private Enemy_DeathBringer enemy;
	private Transform player;
	private int moveDir;
	public DeathBringerBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void Enter()
	{
		base.Enter();
		player = PlayerManager.instance.player.transform;

		/*if (player.GetComponent<PlayerStats>().isDead)
			stateMachine.ChangeState(enemy.moveState);*/
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();



		if (enemy.IsPlayerDetected())
		{
			stateTimer = enemy.battleTime;
			if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
			{
				if (CanAttack())
					stateMachine.ChangeState(enemy.attackState);
			}
		}
		else
		{
			if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 7)
				stateMachine.ChangeState(enemy.idleState);
		}

		if (player.position.x > enemy.transform.position.x)
			moveDir = 1;
		else if (player.position.x < enemy.transform.position.x)
			moveDir = -1;

		enemy.SetVelocity(enemy.moveSpeed * moveDir, enemy.rb.velocity.y);
	}
	private bool CanAttack()
	{
		if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
		{
			enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
			enemy.lastTimeAttacked = Time.time;
			return true;
		}
		return false;
	}
}