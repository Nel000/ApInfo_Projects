using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MealPreparation : MonoBehaviour
{
    [SerializeField] 
    private List<GameObject> waitSlots = new List<GameObject>();
    public List<GameObject> WaitSlots 
    { 
        get => waitSlots; 
        set => waitSlots = value; 
    }

    [SerializeField] private Image[] mealImages;

    [SerializeField] private GameObject waitSlotPrefab;

    public void AddMeal(int i, int prepText)
    {
        foreach (GameObject slot in waitSlots)
        {
            slot.transform.position = new Vector3(
                slot.transform.position.x, slot.transform.position.y + 40,
                slot.transform.position.z);
        }

        GameObject slotObject = Instantiate(
            waitSlotPrefab, gameObject.transform);
        slotObject.GetComponent<WaitingSlot>().DefineMeal(
            mealImages[i], prepText);
        waitSlots.Add(slotObject);
    }

    public void RemoveMeal(GameObject currentMeal)
    {
        for (int i = 0; i < waitSlots.Count; i++)
        {
            if (waitSlots.IndexOf(waitSlots[i]) < waitSlots.IndexOf(currentMeal))
            {
                waitSlots[i].transform.position = new Vector3(
                    waitSlots[i].transform.position.x, 
                    waitSlots[i].transform.position.y - 40,
                    waitSlots[i].transform.position.z);
            }
        }
        waitSlots.Remove(currentMeal);
    }
}
