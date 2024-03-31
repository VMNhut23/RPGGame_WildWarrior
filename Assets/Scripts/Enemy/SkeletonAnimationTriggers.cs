using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationTriggers : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();
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
			{
				PlayerStats _target = hit.GetComponent<PlayerStats>();

				enemy.stats.DoDamage(_target);
			}
		}
	}
	private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
	private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
