using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerFX : EntityFX
{
	[Header("Screen shake FX")]
	private CinemachineImpulseSource screenShake;
	[SerializeField] private float shakeMultiplier;
	public Vector3 shakeSwordImpact;
	public Vector3 shakeHighDamage;

	[Header("After image FX")]
	[SerializeField] private GameObject afterImagePrefab;
	[SerializeField] private float colorLooseRate;
	[SerializeField] private float afterImageCooldown;
	private float afterImageCooldownTimer;

	[Header("DustFX")]
	[SerializeField] private ParticleSystem dustFX;

	protected override void Start()
	{
		base.Start();
		screenShake = GetComponent<CinemachineImpulseSource>();
	}
	private void Update()
	{
		afterImageCooldownTimer -= Time.deltaTime;
	}
	public void CreateAfterImage()
	{
		if (afterImageCooldownTimer < 0)
		{
			afterImageCooldownTimer = afterImageCooldown;
			GameObject newAfterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
			newAfterImage.GetComponent<AfterImageFx>().SetupAfterImage(colorLooseRate, sr.sprite);
		}
	}
	public void ScreenShake(Vector3 _shakePower)
	{
		screenShake.m_DefaultVelocity = new Vector3(_shakePower.x * PlayerManager.instance.player.facingDir, _shakePower.y) * shakeMultiplier;
		screenShake.GenerateImpulse();
	}
	public void PlayDustFX()
	{
		if (dustFX != null)
			dustFX.Play();
	}
}
