using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim;
	public string id;
	public bool activationStatus;
	private void Start()
	{
		anim = GetComponent<Animator>();
	}
	[ContextMenu("Generate checkpoint id")]
	public void GenerateID()
	{
		id = System.Guid.NewGuid().ToString();
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.GetComponent<Player>() != null)
		{
			ActiveCheckpoint();
		}
	}

	public void ActiveCheckpoint()
	{
		if(activationStatus == false)
			AudioManager.instance.PlaySFX(5, transform);
		activationStatus = true;
		anim.SetBool("active", true);
	}
}
