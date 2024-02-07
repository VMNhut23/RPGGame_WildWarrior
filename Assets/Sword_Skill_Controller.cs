using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
	[SerializeField] private float returnSpeed = 12;
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider2;
    private Player player;

	private bool canRotate = true;
	private bool isReturning;
	private void Awake()
	{
		anim = GetComponentInChildren<Animator>();
		rb = GetComponent<Rigidbody2D>();
		circleCollider2 = GetComponent<CircleCollider2D>();
	}
	public void SetUpClone(Vector2 _dir, float gravityScale, Player _player)
	{
		player = _player;
		rb.velocity = _dir;
		rb.gravityScale = gravityScale;

		anim.SetBool("Rotation", true);
	}
	public void ReturnSword()
	{
		rb.constraints = RigidbodyConstraints2D.FreezeAll;
		//rb.isKinematic = false;
		transform.parent = null;
		isReturning = true;
	}
	private void Update()
	{
		if(canRotate)
			transform.right = rb.velocity;

		if (isReturning)
		{
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

			if (Vector2.Distance(transform.position, player.transform.position) < 1)
				player.CatchTheSword();
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (isReturning)
			return;

		anim.SetBool("Rotation", false);
		canRotate = false;
		circleCollider2.enabled = false;

		rb.isKinematic = true;
		rb.constraints = RigidbodyConstraints2D.FreezeAll;

		transform.parent = collision.transform;

	}
}
