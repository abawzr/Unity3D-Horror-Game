using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class InteractionUI : MonoBehaviour
{
    private TMP_Text _interactionText;

    private void Awake()
    {
        _interactionText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        PlayerInteraction.OnInteractableObjectLook += UpdateUI;
    }

    private void OnDisable()
    {
        PlayerInteraction.OnInteractableObjectLook -= UpdateUI;
    }

    private void UpdateUI(string prompt)
    {
        _interactionText.text = prompt;
    }
}
