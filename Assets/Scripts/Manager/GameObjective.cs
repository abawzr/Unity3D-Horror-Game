using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjective : MonoBehaviour
{
    [SerializeField] private List<ObjectiveItem> neededItems;
    [SerializeField] private List<ObjectiveItem> collectedItems;

    public static event Action<int, int> OnObjectiveProgress;
    public static event Action OnObjectiveCompleted;

    private void OnEnable()
    {
        foreach (ObjectiveItem item in neededItems)
        {
            item.OnObjectiveItemCollected += CollectObjectiveItem;
        }
    }

    private void OnDisable()
    {
        foreach (ObjectiveItem item in neededItems)
        {
            item.OnObjectiveItemCollected -= CollectObjectiveItem;
        }
    }

    private void CollectObjectiveItem(ObjectiveItem objectiveItem)
    {
        neededItems.Remove(objectiveItem);
        collectedItems.Add(objectiveItem);

        OnObjectiveProgress?.Invoke(CollectedItems(), TotalItems());

        if (CollectedItems() == TotalItems()) OnObjectiveCompleted?.Invoke();
    }

    private int TotalItems() => collectedItems.Count + neededItems.Count;
    private int CollectedItems() => collectedItems.Count;
}
