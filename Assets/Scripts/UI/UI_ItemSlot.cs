using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;

	protected UI ui;
    public InventoryItem item;
	protected virtual void Start()
	{
		ui = GetComponentInParent<UI>();
	}
	public void UpdateSlot(InventoryItem _newItem)
	{
		item = _newItem;
		itemImage.color = Color.white;
		if (item != null)
		{
			itemImage.sprite = item.itemData.itemIcon;
			if(item.stackSize > 1)
			{
				itemText.text = item.stackSize.ToString();
			}
			else
			{
				itemText.text = "";
			}
		}
	}
	public void CleanUpSlot()
	{
		item = null;
		itemImage.sprite = null;
		itemImage.color = Color.clear;
		itemText.text = "";
	}

	public virtual void OnPointerDown(PointerEventData eventData)
	{
		if (item == null)
			return;

		if (Input.GetKey(KeyCode.LeftControl))
		{
			Inventory.instance.RemoveItem(item.itemData);
			return;
		}

		if (item.itemData.itemType == ItemType.Equipment)
			Inventory.instance.EquipItem(item.itemData);

		ui.itemTooltip.HideTooltip();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (item == null)
			return;
		ui.itemTooltip.ShowTooltip(item.itemData as ItemData_Equipment);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (item == null)
			return;
		ui.itemTooltip.HideTooltip();
	}
}
