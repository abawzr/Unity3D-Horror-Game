using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class WinLoseUI : MonoBehaviour
{
    [SerializeField] private string winTextToDisplay;
    [SerializeField] private string loseTextToDisplay;

    private TMP_Text _winLoseText;

    private void Awake()
    {
        _winLoseText = GetComponent<TMP_Text>();
    }

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

    private void UpdateUI(bool isWin)
    {
        if (isWin) _winLoseText.text = winTextToDisplay;
        else _winLoseText.text = loseTextToDisplay;
    }
}
