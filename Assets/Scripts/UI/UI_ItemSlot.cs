using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

	private UI ui;
    public InventoryItem item;
	private void Start()
	{
		ui = GetComponentInParent<UI>();
	}
	public void UpdateSlot(InventoryItem _newItem)
	{
		item = _newItem;
		itemImage.color = Color.white;
		if (item != null)
		{
			itemImage.sprite = item.itemData.icon;
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
