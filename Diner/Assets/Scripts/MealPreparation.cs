using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MealPreparation : MonoBehaviour
{
    [SerializeField] 
    private List<GameObject> waitSlots = new List<GameObject>();

    [SerializeField] private Image[] mealImages;

    [SerializeField] private GameObject waitSlotPrefab;

    public void AddMeal(int i, int prepText)
    {
        foreach (GameObject slot in waitSlots)
        {
            slot.transform.position = new Vector3(
                slot.transform.position.x, slot.transform.position.y + 70,
                slot.transform.position.z);
        }

        // Add meal to list and instantiate waiting slot with properties
        GameObject slotObject = Instantiate(
            waitSlotPrefab, gameObject.transform);
        slotObject.GetComponent<WaitingSlot>().DefineMeal(
            mealImages[i], prepText);
        waitSlots.Add(slotObject);
    }
}
