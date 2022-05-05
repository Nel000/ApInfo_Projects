using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public bool IsEmpty { get; set; }

    [SerializeField] private Image[] mealImages;

    [SerializeField] private Image slotImage;
    public Image SlotImage { get => slotImage; }

    private void Start()
    {
        IsEmpty = true;
    }

    public void UpdateInventory(int i)
    {
        Debug.Log("Updated inventory slot.");

        if (IsEmpty)
        {
            slotImage = mealImages[i];
            mealImages[i].enabled = true;
            IsEmpty = false;
        }
        else
        {
            for (int j = 0; j < mealImages.Length; j++)
                mealImages[j].enabled = false;
            slotImage = null;
            IsEmpty = true;
        }
    }
}
