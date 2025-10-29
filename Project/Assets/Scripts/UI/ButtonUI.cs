using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] AudioClip clickClip;
    [SerializeField] AudioClip hoverClip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play2DSFX(hoverClip, volume: 0.2f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.Play2DSFX(clickClip, volume: 0.2f);
        }
    }
}