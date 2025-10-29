using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            if (startButton != null)
                startButton.onClick.AddListener(GameManager.Instance.StartGame);

            if (quitButton != null)
                quitButton.onClick.AddListener(GameManager.Instance.QuitGame);
        }
    }
}
