using UnityEngine;

public class PlacementPoint : MonoBehaviour
{
    [SerializeField] private Balcony balcony;

    public bool IsOccupied { get; set; }

    public GameObject PlacedMeal { get; set; }

    public int MealIndex { get; set; }

    private void OnMouseDown()
    {
        if (!FindObjectOfType<GameManager>().IsLocked && PlacedMeal != null
            && balcony.IsOnBalcony)
        {
            IsOccupied = false;
            Destroy(PlacedMeal);

            balcony.Waiter.AddToInventory(MealIndex);
        }
    }
}
