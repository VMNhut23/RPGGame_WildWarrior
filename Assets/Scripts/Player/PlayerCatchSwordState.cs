using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatchSwordState : PlayerState
{
	private Transform sword;
	public PlayerCatchSwordState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
	{
	}

	public override void Enter()
	{
		base.Enter();

		sword = player.sword.transform;

		player.entityFX.PlayDustFX();
		player.entityFX.ScreenShake(player.entityFX.shakeSwordImpact);

		if (player.transform.position.x > sword.position.x && player.facingDir == 1)
			player.Flip();
		else if (player.transform.position.x < sword.position.x && player.facingDir == -1)
			player.Flip();

		player.rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir, player.rb.velocity.y);
	}

	public override void Exit()
	{
		base.Exit();
		player.StartCoroutine("BusyFor", .1f);
	}

	public override void Update()
	{
		base.Update();

		if (triggerCalled)
			stateMachine.ChangeState(player.idleState);
	}
}
