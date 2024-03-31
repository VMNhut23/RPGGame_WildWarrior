using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemyBase;

    protected bool triggerCalled;
    private string animBoolName;

    protected float stateTimer;

    public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
	{
        this.enemyBase = _enemyBase;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
	}
    public virtual void Enter()
	{
        triggerCalled = false;
        enemyBase.animator.SetBool(animBoolName, true);
	}
    public virtual void Update()
	{
        stateTimer -= Time.deltaTime;
	}
    public virtual void Exit()
	{
        enemyBase.animator.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
	}
    public virtual void AnimationFinishTrigger()
	{
        triggerCalled = true;
	}
}
