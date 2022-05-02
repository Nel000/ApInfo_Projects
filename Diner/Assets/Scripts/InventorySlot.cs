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

    public void UpdateInventory(Image mealImage)
    {
        Debug.Log("Updated inventory slot.");
        slotImage = Instantiate(mealImage, gameObject.transform);
    }
}
