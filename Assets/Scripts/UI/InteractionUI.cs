using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class InteractionUI : MonoBehaviour
{
    private TMP_Text _interactionText;

    private void Awake()
    {
        _interactionText = GetComponent<TMP_Text>();
        ClearText();
    }

    public void SetText(string newText)
    {
        _interactionText.text = newText;
    }

    public void ClearText()
    {
        _interactionText.text = string.Empty;
    }
}
