using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{
	private Animator anim => GetComponent<Animator>();
	private CircleCollider2D cd => GetComponent<CircleCollider2D>();

    private float crystalExitTimer;

	private bool canExlpode;
	private bool canMove;
	private float moveSpeed;

	private bool canGrow;
	[SerializeField] private float growSpeed;

	private Transform closestTarget;
    public void SetupCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestTarget)
	{
		crystalExitTimer = _crystalDuration;
		canExlpode = _canExplode;
		canMove = _canMove;
		moveSpeed = _moveSpeed;
		closestTarget = _closestTarget;

	}
	private void Update()
	{
		crystalExitTimer -= Time.deltaTime;

		if(crystalExitTimer < 0)
		{
			FinishCrystal();
		}

		if (canMove)
		{
			transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);

			if (Vector2.Distance(transform.position, closestTarget.position) < 1)
			{
				FinishCrystal();
				canMove = false;
			}
				
		}
		if (canGrow)
			transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);

	}
	private void AnimationExplodeEvent()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<Enemy>() != null)
				hit.GetComponent<Enemy>().Damage();
		}
	}
	public void FinishCrystal()
	{
		if (canExlpode)
		{
			canGrow = true;
			anim.SetTrigger("Explode");
		}
		else
			SelfDestroy();
	}

	public void SelfDestroy() => Destroy(gameObject);
}
