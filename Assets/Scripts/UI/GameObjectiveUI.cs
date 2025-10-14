using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GameObjectiveUI : MonoBehaviour
{
    [SerializeField] private int defaultCollectedItems = 0;
    [SerializeField] private int defaultTotalItems = 10;

    private TMP_Text _objectiveText;

    private void OnEnable()
    {
        GameObjective.OnObjectiveProgress += UpdateUI;
    }

    private void OnDisable()
    {
        GameObjective.OnObjectiveProgress -= UpdateUI;
    }

    private void Awake()
    {
        _objectiveText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        UpdateUI(defaultCollectedItems, defaultTotalItems);
    }

    private void UpdateUI(int collectedItems, int totalItems)
    {
        _objectiveText.text = $"Collected Items {collectedItems} / {totalItems}";
    }
}
