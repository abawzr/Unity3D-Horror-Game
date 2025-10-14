using System;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private ItemSO item;

    public string InteractionPrompt { get; set; }

    private void Awake()
    {
        InteractionPrompt = $"Pick Up {item.itemName}";
    }

    public void Interact()
    {
        if (playerInventory != null && !playerInventory.IsInventoryFull())
        {
            playerInventory.AddItem(item);
            gameObject.SetActive(false);
        }
    }
}
