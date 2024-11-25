using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
	private Entity entity => GetComponentInParent<Entity>();
	private CharacterStats myStats => GetComponentInParent<CharacterStats>();
	private RectTransform myTransform;
	private Slider slider;
	private void Start()
	{
		myTransform = GetComponent<RectTransform>();
		slider = GetComponentInChildren<Slider>();

		
		UpdateHealthUI();
	}
	private void OnEnable()
	{
		entity.onFlipped += FilpUI;
		myStats.onHealthChanged += UpdateHealthUI;

	}
	private void UpdateHealthUI()
	{
		slider.maxValue = myStats.GetMaxHealthValue();
		slider.value = myStats.currentHealth;
	}
	private void OnDisable()
	{
		if(entity != null)
			entity.onFlipped -= FilpUI;

		if(myStats != null)
			myStats.onHealthChanged -= UpdateHealthUI;
	}
	private void FilpUI() => myTransform.Rotate(0, 180, 0);

}
