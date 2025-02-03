using UnityEngine;


public class DeathBringerDeadState : EnemyState
{
    private Enemy_DeathBringer enemy;

    public DeathBringerDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_DeathBringer enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.animator.SetBool(enemy.lastAnimBoolName, true);
        enemy.animator.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .15f;
        GameObject.Find("Canvas").GetComponent<UI>().SwitchOnWinScreen();
    }
    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
            enemy.rb.velocity = new Vector2(0, 10);

        AudioManager.instance.StopBGM(1);
    }
}
