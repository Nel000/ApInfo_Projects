using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public bool IsEmpty { get; set; }

    [SerializeField] private Image[] mealImages;
    public Image[] MealImages { get => mealImages; }

    [SerializeField] private Image slotImage;
    public Image SlotImage { get => slotImage; }

    private void Start()
    {
        IsEmpty = true;
    }

    public void UpdateInventory(Image mealImage)
    {
        Debug.Log("Updated inventory slot.");

        if (IsEmpty)
        {
            slotImage = Instantiate(mealImage, gameObject.transform);
            IsEmpty = false;
        }
        else
        {
            // Should remove current image
            slotImage = null;
            IsEmpty = true;
        }
    }
}
