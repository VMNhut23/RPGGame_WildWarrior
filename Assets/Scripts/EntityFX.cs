using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

	[Header("Flash FX")]
	[SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originMat;
	private void Start()
	{
		sr = GetComponent<SpriteRenderer>();
		originMat = sr.material;
	}
	IEnumerator FlashFX()
	{
		sr.material = hitMat;
		yield return new WaitForSeconds(flashDuration);
		sr.material = originMat;
	}
}
