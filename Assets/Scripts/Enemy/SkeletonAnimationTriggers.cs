using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponent<Enemy_Skeleton>();
    private void AnimationTrigger()
	{
		enemy.AnimationFinishTrigger();
	}
	private void AttackTrigger()
	{
		Collider2D[] collider2D = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
		foreach (var hit in collider2D)
		{
			if (hit.GetComponent<Player>() != null)
				hit.GetComponent<Player>().Damage();
		}
	}
	private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
	private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
