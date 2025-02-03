using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTrigger : MonoBehaviour
{
    public GameObject map;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			map.SetActive(true);
			AudioManager.instance.PlayBGM(1);
			gameObject.SetActive(false);
		}
	}
}
