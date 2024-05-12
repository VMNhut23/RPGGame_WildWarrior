using System.Text;
using UnityEngine;

public enum ItemType
{
	Material,
	Equipment
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
	public ItemType itemType;
	public string itemName;
	public Sprite itemIcon;

	[Range(0,100)]
	public float dropChange;

	protected StringBuilder sb = new StringBuilder();

	public virtual string GetDescription()
	{
		return "";
	}
}
