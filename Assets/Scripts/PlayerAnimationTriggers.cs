using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    public Player player => GetComponent<Player>();
    public void AnimationTrigger()
	{
		player.AnimationTrigger();
	}
}
