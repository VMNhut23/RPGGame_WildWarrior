using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
	public int damage = 10;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			CharacterStats characterStats = collision.GetComponent<CharacterStats>();
			if(characterStats != null)
			{
				characterStats.TakeDamage(damage);
			}
		}
	}
}
