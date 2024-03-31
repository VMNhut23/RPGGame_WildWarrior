using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharacterStats : MonoBehaviour
{
	[Header("Major stats")]
	public Stat strength;//suc manh
	public Stat agility;//nhanh nhen
	public Stat intelligence;//thong minh;
	public Stat vitality;//sinh luc

	[Header("Offensive stats")]
	public Stat damage;
	public Stat critChance;
	public Stat critPower; // default value 150%

	[Header("Defensive stats")]
	public Stat maxHealth;
	public Stat armour;//giap
	public Stat evasion;//ne tranh;
	public Stat magicResistance;


	[Header("Magic stats")]
	public Stat fireDamage;
	public Stat iceDamage;
	public Stat lightingDamage;

	public bool isIgnited;
	public bool isChilled;
	public bool isShocked;



	[SerializeField] private int currentHealth;
	protected virtual void Start()
	{
		critPower.SetDefaultValue(150);
		currentHealth = maxHealth.GetValue();
	}
	public virtual void DoDamage(CharacterStats _targetStats)
	{
		if (TargetCanAvoidAttack(_targetStats))
			return;

		int totalDamage = damage.GetValue() + strength.GetValue();

		if (CanCrit())
		{
			totalDamage = CalculateCriticalDamage(totalDamage);
			Debug.Log("Total crit damage is" + totalDamage);
		}

		totalDamage = CheckTargerArmour(_targetStats, totalDamage);
		//_targetStats.TakeDamage(totalDamage);
		DoMagicalDamage(_targetStats);
	}
	public virtual void DoMagicalDamage(CharacterStats _targetStats)
	{
		int _fireDamage = fireDamage.GetValue();
		int _iceDamage = iceDamage.GetValue();
		int _lightingDamage = lightingDamage.GetValue();

		int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
		totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

		_targetStats.TakeDamage(totalMagicalDamage);

		if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
			return;
		bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
		bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
		bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

		while(!canApplyIgnite && !canApplyChill && !canApplyShock)
		{
			if(Random.value < .5f && _fireDamage > 0)
			{
				canApplyIgnite = true;
				_targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
				Debug.Log("Applied fire");
				return;
			}
			if (Random.value < .5f && _iceDamage > 0)
			{
				canApplyChill = true;
				_targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
				Debug.Log("Applied ice");
				return;
			}
			if (Random.value < .5f && _lightingDamage > 0)
			{
				canApplyShock = true;
				_targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
				Debug.Log("Applied lighting");
				return;
			}
		}

		_targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);
	}

	private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
	{
		totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
		totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
		return totalMagicalDamage;
	}

	public void ApplyAliments(bool _ignite, bool _chill, bool _shock)
	{
		if (isIgnited || isChilled || isShocked)
			return;

		isIgnited = _ignite;
		isChilled = _chill;
		isShocked = _shock;
	}


	public virtual void TakeDamage(int _damage)
	{
		currentHealth -= _damage;

		Debug.Log(_damage);
		if (currentHealth <= 0)
			Die();
	}

	protected virtual void Die()
	{
		//throw new NotImplementedException();
	}
	private int CheckTargerArmour(CharacterStats _targetStats, int totalDamage)
	{
		totalDamage -= _targetStats.armour.GetValue();
		totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
		return totalDamage;
	}
	private bool TargetCanAvoidAttack(CharacterStats _targetStats)
	{
		int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
		if (isShocked)
			totalEvasion += 20;

		if (Random.Range(0, 100) < totalEvasion)
		{
			return true;
		}
		return false;
	}
	private bool CanCrit()
	{
		int totalCriticalChane = critChance.GetValue() + agility.GetValue();

		if(Random.Range(0,100) <= totalCriticalChane)
		{
			return true;
		}
		return false;
	}
	private int CalculateCriticalDamage(int _damage)
	{
		float totalCritPower = (critPower.GetValue() + strength.GetValue()) * .01f;
		float critDamage = _damage * totalCritPower;
		return Mathf.RoundToInt(critDamage);
	}
}
