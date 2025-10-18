using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class GameObjectiveUI : MonoBehaviour
{
    private TMP_Text _objectiveText;

    private void Awake()
    {
        _objectiveText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        GameObjective.OnObjectiveProgress += UpdateUI;
    }

    private void OnDisable()
    {
        GameObjective.OnObjectiveProgress -= UpdateUI;
    }

    private void UpdateUI(int collectedItems, int totalItems)
    {
        _objectiveText.text = $"Collected Items {collectedItems} / {totalItems}";
    }
}
