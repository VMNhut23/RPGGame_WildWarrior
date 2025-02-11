using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerState
{
	private bool canCreateClone;
	public PlayerCounterAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		canCreateClone = true;
		stateTimer = player.counterAttackDuration;
		player.animator.SetBool("SuccessCounterAttack", false);
	}

	public override void Exit()
	{
		base.Exit();
	}

	public override void Update()
	{
		base.Update();
		player.ZeroVelocity();

		Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
		foreach (var hit in colliders)
		{

			if (hit.GetComponent<Arrow_Controller>() != null)
			{
				hit.GetComponent<Arrow_Controller>().FlipArrow();
				SuccessCounterAttack();
			}

			if (hit.GetComponent<Enemy>() != null)
			{
				if (hit.GetComponent<Enemy>().CanBeStunned())
					{
						SuccessCounterAttack();

						player.skill.parry.UseSkill();

						if (canCreateClone)
						{
							canCreateClone = false;
							player.skill.parry.MakeMirageOnParry(hit.transform);
						}
					}
				}	
		}

		if (stateTimer < 0 || triggerCalled)
			stateMachine.ChangeState(player.idleState);
	}

	private void SuccessCounterAttack()
	{
		stateTimer = 10;
		player.animator.SetBool("SuccessCounterAttack", true);
	}
}
