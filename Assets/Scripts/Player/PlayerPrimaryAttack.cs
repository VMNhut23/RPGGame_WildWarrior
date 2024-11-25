using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttack : PlayerState
{
	public int comboCounter { get; private set; }

	private float latsTimeAttacked;
	private float comboWindow = 2;
	public PlayerPrimaryAttack(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		if (comboCounter > 2 || Time.time >= latsTimeAttacked + comboWindow)
			comboCounter = 0;

		player.animator.SetInteger("ComboCounter", comboCounter);
		player.animator.speed = 0.9f;

		float attackDir = player.facingDir;
		if (xInput != 0)
			attackDir = xInput;

		player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
		stateTimer = .1f;
	}

	public override void Exit()
	{
		base.Exit();

		player.StartCoroutine("BusyFor", .15f);
		player.animator.speed = 1;
		comboCounter++;
		latsTimeAttacked = Time.time;
	}

	public override void Update()
	{
		base.Update();

		if (stateTimer < 0)
			player.ZeroVelocity();

		if (triggerCalled)
			stateMachine.ChangeState(player.idleState);
	}
}
