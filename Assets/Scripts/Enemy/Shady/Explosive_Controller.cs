using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive_Controller : MonoBehaviour
{
	private Animator amin;
	private CharacterStats myStats;
	private float growSpeed = 15;
	private float maxSize = 6;
	private float explosionRadius;

	private bool canGrow = true;
	private void Update()
	{
		if (canGrow)
			transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

		if(maxSize - transform.localScale.x < 1.5f)
		{
			canGrow = false;
			amin.SetTrigger("Explode");
		}
	}
	public void SetupExplosive(CharacterStats _myStats, float _growSpeed, float _maxSize, float _radius)
	{
		amin = GetComponent<Animator>();
		myStats = _myStats;
		growSpeed = _growSpeed;
		maxSize = _maxSize;
		explosionRadius = _radius;
	}

	private void AnimationExplodeEvent()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
		foreach (var hit in colliders)
		{
			if (hit.GetComponent<Enemy>() != null)
			{
				hit.GetComponent<Entity>().SetupKnockbackDir(transform);
				myStats.DoDamage(hit.GetComponent<CharacterStats>());

			}
		}
	}
	private void SelfDestroy() => Destroy(gameObject);
}
