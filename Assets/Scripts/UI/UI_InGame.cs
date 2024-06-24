using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_InGame : MonoBehaviour
{
	[SerializeField] private PlayerStats playerStats;
	[SerializeField] private Slider slider;

	[SerializeField] private Image dashImage;
	[SerializeField] private Image parryImage;
	[SerializeField] private Image crystalImage;
	[SerializeField] private Image swordImage;
	[SerializeField] private Image blackholeImage;
	[SerializeField] private Image flaskImage;
	private SkillManager skill; 

	[Header("Souls info")]
	[SerializeField] private TextMeshProUGUI currentSouls;
	[SerializeField] private float soulsAmount;
	[SerializeField] private float increaseRate = 100;
	private void Start()
	{
		if (playerStats != null)
			playerStats.onHealthChanged += UpdateHealthUI;

		skill = SkillManager.instance;
	}
	private void Update()
	{
		UpdateSoulsUI();

		if (Input.GetKeyDown(KeyCode.LeftShift) && skill.dash.dashUnlocked)
			SetCooldownOf(dashImage);
		if (Input.GetKeyDown(KeyCode.Q) && skill.parry.parryUnlocked)
			SetCooldownOf(parryImage);
		if (Input.GetKeyDown(KeyCode.F) && skill.crystal.crystalUnlocked)
			SetCooldownOf(crystalImage);
		if (Input.GetKeyDown(KeyCode.Mouse1) && skill.sword.swordUnlocked)
			SetCooldownOf(swordImage);
		if (Input.GetKeyDown(KeyCode.R) && skill.blackhole.blackHoleUnlocked)
			SetCooldownOf(blackholeImage);
		if (Input.GetKeyDown(KeyCode.H) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
			SetCooldownOf(flaskImage);

		CheckCooldownOf(dashImage, skill.dash.cooldown);
		CheckCooldownOf(parryImage, skill.parry.cooldown);
		CheckCooldownOf(crystalImage, skill.crystal.cooldown);
		CheckCooldownOf(swordImage, skill.sword.cooldown);
		CheckCooldownOf(blackholeImage, skill.blackhole.cooldown);
		CheckCooldownOf(flaskImage, Inventory.instance.flaskCooldown);

	}

	private void UpdateSoulsUI()
	{
		if (soulsAmount < PlayerManager.instance.GetCurrency())
			soulsAmount += Time.deltaTime * increaseRate;
		else
			soulsAmount = PlayerManager.instance.GetCurrency();

		currentSouls.text = ((int)soulsAmount).ToString();
	}

	private void UpdateHealthUI()
	{
		slider.maxValue = playerStats.GetMaxHealthValue();
		slider.value = playerStats.currentHealth;
	}
	private void SetCooldownOf(Image _image)
	{
		if (_image.fillAmount <= 0)
			_image.fillAmount = 1;
	}
	private void CheckCooldownOf(Image _image, float _cooldown)
	{
		if (_image.fillAmount > 0)
			_image.fillAmount -= 1 / _cooldown * Time.deltaTime;
	}
}
