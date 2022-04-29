using UnityEngine;

public class PlacementPoint : MonoBehaviour
{
    public bool IsOccupied { get; set; }

    public GameObject PlacedMeal { get; set; }

    private void OnMouseDown()
    {
        if (!FindObjectOfType<GameManager>().IsLocked && PlacedMeal != null)
        {
            Destroy(PlacedMeal);
        }
    }
}
