using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private InventorySlotUI[] _inventorySlots = new InventorySlotUI[3];

    private void Start()
    {
        if (playerInventory != null)
        {
            playerInventory.OnItemPicked += AddItem;
            playerInventory.OnItemDropped += RemoveItem;
            playerInventory.OnItemUsed += RemoveItem;
        }
    }

    private void AddItem(ItemSO newItem)
    {
        if (newItem == null) return;

        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            if (_inventorySlots[i].HasItem) continue;

            _inventorySlots[i].SetItem(newItem);
            break;
        }
    }

    private void RemoveItem(ItemSO item)
    {
        if (item == null) return;

        for (int i = 0; i < _inventorySlots.Length; i++)
        {
            if (_inventorySlots[i].HasItem)
            {
                _inventorySlots[i].RemoveItem(item);
                break;
            }
        }
    }
}
