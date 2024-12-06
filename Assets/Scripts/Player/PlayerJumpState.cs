using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
	public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();
		player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
		AudioManager.instance.PlaySFX(36, null);
	}

	public override void Exit()
	{
		base.Exit();
		AudioManager.instance.StopSFX(36);
	}

	public override void Update()
	{
		base.Update();
		if (player.rb.velocity.y < 0)
			stateMachine.ChangeState(player.airState);
	}
}
