using System;
using System.Collections.Generic;
using UnityEngine;

public class GameObjective : MonoBehaviour
{
    [SerializeField] private List<ObjectiveItem> neededItems;
    [SerializeField] private List<ObjectiveItem> collectedItems;

    public static int TotalItems { get; private set; }
    public static int CollectedItems { get; private set; }

    public static event Action<int, int> OnObjectiveProgress;
    public static event Action OnObjectiveCompleted;

    private void Awake()
    {
        TotalItems = collectedItems.Count + neededItems.Count;
        CollectedItems = collectedItems.Count;
    }

    private void OnEnable()
    {
        foreach (ObjectiveItem item in neededItems)
        {
            item.OnObjectiveItemCollected += CollectObjectiveItem;
        }
    }

    private void Start()
    {
        OnObjectiveProgress?.Invoke(CollectedItems, TotalItems);
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
        CollectedItems++;

        OnObjectiveProgress?.Invoke(CollectedItems, TotalItems);

        if (CollectedItems == TotalItems) OnObjectiveCompleted?.Invoke();
    }
}
