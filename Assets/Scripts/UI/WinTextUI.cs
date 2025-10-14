using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class WinText : MonoBehaviour
{
    [SerializeField] private string textToDisplay;

    private TMP_Text _winText;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameWin += UpdateUI;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameWin -= UpdateUI;
        }
    }

    private void Awake()
    {
        _winText = GetComponent<TMP_Text>();
    }

    private void UpdateUI()
    {
        _winText.text = textToDisplay;
    }
}
