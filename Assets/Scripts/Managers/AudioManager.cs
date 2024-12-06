using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

	[SerializeField] private float sfxMinimumDistance;
	[SerializeField] private AudioSource[] sfx;
	[SerializeField] private AudioSource[] bgm;

	public bool playBGM;
	private int bgmIndex;
	private bool canPlaySFX;

	private void Awake()
	{
		if (instance != null)
			Destroy(instance.gameObject);
		else
			instance = this;

		Invoke("AllowSFX",1f);
	}

	private void Update()
	{
		if (!playBGM)
			StopAllBGM();
		else
		{
			if (!bgm[bgmIndex].isPlaying)
				PlayBGM(bgmIndex);
		}
	}
	public void PlaySFX(int _sfxIndex, Transform _source)
	{
		/*if (sfx[_sfxIndex].isPlaying)
			return;*/
		if (canPlaySFX == false)
			return;

		if (_source != null && Vector2.Distance(PlayerManager.instance.player.transform.position, _source.position) > sfxMinimumDistance)
			return;

		if(_sfxIndex < sfx.Length)
		{
			sfx[_sfxIndex].Play();
		}
	}
	public void StopSFX(int _sfxIndex) => sfx[_sfxIndex].Stop();
	public void StopBGM(int _bgmIndex) => bgm[_bgmIndex].Stop();

	public void PlayRandomBGM()
	{
		bgmIndex = Random.Range(0, bgm.Length);
		PlayBGM(bgmIndex);
	}
	public void PlayBGM(int _bgmIndex)
	{
		bgmIndex = _bgmIndex;

		StopAllBGM();
		bgm[_bgmIndex].Play();
	}

	private void StopAllBGM()
	{
		for (int i = 0; i < bgm.Length; i++)
		{
			bgm[i].Stop();
		}
	}
	private void AllowSFX() => canPlaySFX = true;
}
