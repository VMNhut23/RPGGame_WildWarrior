using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
	[SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private ItemData itemData;
	[SerializeField] private Vector2 velocity;
	private void SetupVisuals()
	{
		if (itemData == null)
			return;

		GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
		gameObject.name = "Item object - " + itemData.name;
	}
	public void SetupItem(ItemData _itemData, Vector2 _velocity)
	{
		itemData = _itemData;
		rb2D.velocity = _velocity;

		SetupVisuals();
	}

	public void PickupItem()
	{
		if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
		{
			rb2D.velocity = new Vector2(0, 7);
			PlayerManager.instance.player.entityFX.CreatePopupText("Inventory is full");
			return;
		}
		AudioManager.instance.PlaySFX(18, transform);
		Inventory.instance.AddItem(itemData);
		Destroy(gameObject);
	}
}
