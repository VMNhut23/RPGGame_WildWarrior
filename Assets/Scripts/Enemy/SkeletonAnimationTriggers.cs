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
}
