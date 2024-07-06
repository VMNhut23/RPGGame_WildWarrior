using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

	[Header("Screen shake FX")]
	private CinemachineImpulseSource screenShake;
	[SerializeField] private float shakeMultiplier;
	public Vector3 shakeSwordImpact;
	public Vector3 shakeHighDamage;

	[Header("Popup text")]
	[SerializeField] private GameObject popUpTextPrefabs;

	[Header("After image FX")]
	[SerializeField] private GameObject afterImagePrefab;
	[SerializeField] private float colorLooseRate;
	[SerializeField] private float afterImageCooldown;
	private float afterImageCooldownTimer;

	[Header("Flash FX")]
	[SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originMat;

	[Header("Ailment colors")]
	[SerializeField] private Color[] chillColor;
	[SerializeField] private Color[] igniteColor;
	[SerializeField] private Color[] shockColor;

	[Header("Ailment particles")]
	[SerializeField] private ParticleSystem igniteFx;
	[SerializeField] private ParticleSystem chillFx;
	[SerializeField] private ParticleSystem shockFx;

	[Header("HixFX")]
	[SerializeField] private GameObject hixFx;
	[SerializeField] private GameObject criticalHitFx;

	[Header("DustFX")]
	[SerializeField] private ParticleSystem dustFX;
	private void Start()
	{
		sr = GetComponentInChildren<SpriteRenderer>();
		screenShake = GetComponent<CinemachineImpulseSource>();
		originMat = sr.material;
	}
	private void Update()
	{
		afterImageCooldownTimer -= Time.deltaTime;
	}
	public void ScreenShake(Vector3 _shakePower)
	{
		screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * PlayerManager.instance.player.facingDir, _shakePower.y) * shakeMultiplier;
		screenShake.GenerateImpulse();
	}
	public void CreatePopupText(string _text)
	{
		float randomX = Random.Range(-1, 1);
		float randomY = Random.Range(1, 3);

		Vector3 positionOffset = new Vector3(randomX, randomY,0);

		GameObject newText = Instantiate(popUpTextPrefabs, transform.position + positionOffset, Quaternion.identity);
		newText.GetComponent<TextMeshPro>().text = _text;

	}
	public void CreateAfterImage()
	{
		if(afterImageCooldownTimer < 0)
		{
			afterImageCooldownTimer = afterImageCooldown;
			GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
			newAfterImage.GetComponent<AfterImageFx>().SetupAfterImage(colorLooseRate, sr.sprite);
		}
	}
	IEnumerator FlashFX()
	{
		sr.material = hitMat;
		Color currentColor = sr.color;
		sr.color = Color.white;

		yield return new WaitForSeconds(flashDuration);

		sr.color = currentColor;
		sr.material = originMat;
	}
	private void RedColorBlink()
	{
		if (sr.color != Color.white)
			sr.color = Color.white;
		else
			sr.color = Color.red;
	}
	public void IgniteFxFor(float _seconds)
	{
		igniteFx.Play();

		InvokeRepeating("IgniteColorFx", 0, 0.3f);
		Invoke("CancelColorChange", _seconds);
	}
	public void ChillFxFor(float _seconds)
	{
		chillFx.Play();

		InvokeRepeating("ChillColorFx", 0, .3f);
		Invoke("CancelColorChange", _seconds);
	}
	public void ShockFxFor(float _seconds)
	{
		shockFx.Play();

		InvokeRepeating("ShockColorFx", 0, 0.3f);
		Invoke("CancelColorChange", _seconds);
	}
	private void CancelColorChange()
	{
		CancelInvoke();
		sr.color = Color.white;

		igniteFx.Stop();
		chillFx.Stop();
		shockFx.Stop();
	}

	private void IgniteColorFx()
	{
		if (sr.color != igniteColor[0])
			sr.color = igniteColor[0];
		else
			sr.color = igniteColor[1];
	}
	private void ChillColorFx()
	{
		if (sr.color != chillColor[0])
			sr.color = chillColor[0];
		else
			sr.color = chillColor[1];
	}
	private void ShockColorFx()
	{
		if (sr.color != shockColor[0])
			sr.color = shockColor[0];
		else
			sr.color = shockColor[1];
	}
	public void CreateHitFX(Transform _target, bool _critical)
	{
		float zRotation = Random.Range(-90, 90);
		float xPosition = Random.Range(-.5f, .5f);
		float yPosition = Random.Range(-.5f, .5f);

		GameObject hitPrefab = hixFx;

		if (_critical)
			hitPrefab = criticalHitFx;

		GameObject newHitFx = Instantiate(hitPrefab, _target.position + new Vector3(xPosition,yPosition), Quaternion.identity);

		if (_critical == false)
			newHitFx.transform.Rotate(new Vector3(0, 0, zRotation));
		else
			newHitFx.transform.localScale = new Vector3(GetComponent<Entity>().facingDir, 1, 1);

		Destroy(newHitFx, .5f);
	}
	public void PlayDustFX()
	{
		if (dustFX != null)
			dustFX.Play();
	}
}
