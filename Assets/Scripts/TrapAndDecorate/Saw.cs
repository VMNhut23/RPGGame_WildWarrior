using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public int damage = 30;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		CharacterStats characterStats = collision.GetComponent<CharacterStats>();
		if(characterStats != null)
		{
			characterStats.TakeDamage(damage);
		}
	}
}
