using UnityEngine;

public enum EquipmentType
{
	Weapon,
	Armor,
	Amulet,
	Flask
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
	public EquipmentType equipmentType;

	[Header("Magic stats")]
	public int strength;
	public int agility;
	public int intelligence;
	public int vitality;

	[Header("Offensive stats")]
	public int damage;
	public int critChance;
	public int critPower;

	[Header("Defensive stats")]
	public int health;
	public int armor;
	public int evasion;
	public int magicResistance;

	[Header("Magic stats")]
	public int fireDamage;
	public int iceDamage;
	public int lightingDamage;

	public void AddModifiers()
	{
		PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

		playerStats.strength.AddModifier(strength);
		playerStats.agility.AddModifier(agility);
		playerStats.intelligence.AddModifier(intelligence);
		playerStats.vitality.AddModifier(vitality);

		playerStats.damage.AddModifier(damage);
		playerStats.critChance.AddModifier(critChance);
		playerStats.critPower.AddModifier(critPower);

		playerStats.maxHealth.AddModifier(health);
		playerStats.armour.AddModifier(armor);
		playerStats.evasion.AddModifier(evasion);
		playerStats.magicResistance.AddModifier(magicResistance);

		playerStats.fireDamage.AddModifier(fireDamage);
		playerStats.iceDamage.AddModifier(iceDamage);
		playerStats.lightingDamage.AddModifier(lightingDamage);
	}
	public void RemoveModifiers()
	{
		PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

		playerStats.strength.RemoveModifier(strength);
		playerStats.agility.RemoveModifier(agility);
		playerStats.intelligence.RemoveModifier(intelligence);
		playerStats.vitality.RemoveModifier(vitality);

		playerStats.damage.RemoveModifier(damage);
		playerStats.critChance.RemoveModifier(critChance);
		playerStats.critPower.RemoveModifier(critPower);

		playerStats.maxHealth.RemoveModifier(health);
		playerStats.armour.RemoveModifier(armor);
		playerStats.evasion.RemoveModifier(evasion);
		playerStats.magicResistance.RemoveModifier(magicResistance);

		playerStats.fireDamage.AddModifier(fireDamage);
		playerStats.iceDamage.AddModifier(iceDamage);
		playerStats.lightingDamage.AddModifier(lightingDamage);
	}
}