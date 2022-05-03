using UnityEngine;

public class PlacementPoint : MonoBehaviour
{
    [SerializeField] private Balcony balcony;
    [SerializeField] private InventorySlot slot;

    public bool IsOccupied { get; set; }

    public GameObject PlacedMeal { get; set; }

    public int MealIndex { get; set; }

    private void OnMouseDown()
    {
        if (!FindObjectOfType<GameManager>().IsLocked && PlacedMeal != null
            && balcony.IsOnBalcony && slot.IsEmpty)
        {
            IsOccupied = false;
            Destroy(PlacedMeal);

            balcony.Waiter.AddToInventory(MealIndex);
        }
    }
}
