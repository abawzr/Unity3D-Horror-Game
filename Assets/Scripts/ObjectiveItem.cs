using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveItem : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO item;

    public string InteractionPrompt { get; set; }
    public event Action<ObjectiveItem> OnObjectiveItemCollected;

    private void Awake()
    {
        InteractionPrompt = $"Collect {item.itemName}";
    }

    public void Interact()
    {
        OnObjectiveItemCollected?.Invoke(this);
        gameObject.SetActive(false);
    }
}
