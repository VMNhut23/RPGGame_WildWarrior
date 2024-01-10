using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attack details")]
    public Vector2[] attackMovement;
    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed = 10f;
    public float jumpForce;

    [Header("Dash info")]
    [SerializeField] private float dashCooldown;
    private float dashTimer;
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }

    [Header("Collision info")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    private bool facingRight = true;

	#region Components
	public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
	#endregion
	#region States
	public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerWallSlideState wallSlide { get; private set; }
    public PlayerWallJumpState wallJump { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerPrimaryAttack primaryAttack { get; private set; }
	#endregion
	private void Awake()
	{
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlide = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJump = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttack(this, stateMachine, "Attack");
	}
	void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine.Initialize(idleState);
    }
	private void Update()
	{
        stateMachine.currentState.Update();
        CheckForDashInput();
	}
    public IEnumerator BusyFor(float _seconds)
	{
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
	}
    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    private void CheckForDashInput()
	{
        if (IsWallDetected())
            return;

        dashTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer < 0)
		{
            dashTimer = dashCooldown;
            dashDir = Input.GetAxisRaw("Horizontal");
			if (dashDir == 0)
			{
                dashDir = facingDir;
			}
            stateMachine.ChangeState(dashState);
		}
            
    }
	#region Velocity
	public void ZeroVelocity() => rb.velocity = new Vector2(0, 0);
    public void SetVelocity(float xVelocity, float yVelocity)
	{
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);
    }
	#endregion
	#region Collision
	public bool IsGroundedDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    private void OnDrawGizmos()
	{
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));
	}
	#endregion
	#region Flip
	public void Flip()
	{
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
	}
    public void FlipController(float _x)
	{
		if (_x > 0 && !facingRight)
		{
            Flip();
		}
		else if (_x < 0 && facingRight)
		{
            Flip();
		}
	}
	#endregion
}
