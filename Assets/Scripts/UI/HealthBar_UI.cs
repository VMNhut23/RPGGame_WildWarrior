using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
	private CharacterStats myStats;
	private RectTransform myTransform;
	private Slider slider;
	private void Start()
	{
		myTransform = GetComponent<RectTransform>();
		entity = GetComponentInParent<Entity>();
		slider = GetComponentInChildren<Slider>();
		myStats = GetComponentInParent<CharacterStats>();

		entity.onFlipped += FilpUI;
		myStats.onHealthChanged += UpdateHealthUI;

		UpdateHealthUI();
	}

	private void UpdateHealthUI()
	{
		slider.maxValue = myStats.GetMaxHealthValue();
		slider.value = myStats.currentHealth;
	}
	private void FilpUI() => myTransform.Rotate(0, 180, 0);
	private void OnDisable()
	{
		entity.onFlipped -= FilpUI;
		myStats.onHealthChanged -= UpdateHealthUI;
	}
	

}
