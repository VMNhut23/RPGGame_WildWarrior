using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBattleState : EnemyState
{
	private Transform player;
	private Enemy_Archer enemy;
	private int moveDir;
	public ArcherBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Archer _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
	{
		this.enemy = _enemy;
	}

	public override void Enter()
	{
		base.Enter();
		player = PlayerManager.instance.player.transform;

		if (player.GetComponent<PlayerStats>().isDead)
			stateMachine.ChangeState(enemy.moveState);
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

			if(enemy.IsPlayerDetected().distance < enemy.safeDistance)
			{
				if (CanJump())
					stateMachine.ChangeState(enemy.jumpState);
			}

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


		//Bc archer remote attack so we don't apply this code
		/*if (player.position.x > enemy.transform.position.x)
			moveDir = 1;
		else if (player.position.x < enemy.transform.position.x)
			moveDir = -1;

		enemy.SetVelocity(enemy.moveSpeed * moveDir, enemy.rb.velocity.y);*/
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
	private bool CanJump()
	{
		if (enemy.GroundBehindCheck() == false || enemy.GroundWallCheck() == true)
			return false;

		if(Time.time >= enemy.lastTimeJumped + enemy.jumpCooldown)
		{
			enemy.lastTimeJumped = Time.time;
			return true;
		}
		return false;
	}
}
