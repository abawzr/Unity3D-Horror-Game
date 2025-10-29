using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text itemName;

    [SerializeField] private Image itemIcon;

    private ItemSO currentItem;

    public bool HasItem { get; private set; } = false;

    public void SetItem(ItemSO newItem)
    {
        if (HasItem) return;

        currentItem = newItem;
        itemIcon.sprite = newItem.itemIcon;
        itemIcon.enabled = true;
        itemIcon.preserveAspect = true;
        itemName.text = newItem.itemName;
        HasItem = true;
    }

    public void RemoveItem(ItemSO item)
    {
        if (currentItem == item)
        {
            currentItem = null;
            itemIcon.sprite = null;
            itemIcon.enabled = false;
            itemName.text = string.Empty;
            HasItem = false;
        }
    }
}
