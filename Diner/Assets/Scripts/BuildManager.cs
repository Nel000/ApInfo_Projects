using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private Balcony balcony;

    private const float floorPosX = -5.5f, floorPosY = -2.75f;
    private const float counterPosX = 0.49f, counterPosY = 0.245f;
    private const float placePointPosX = 0.6f, placePointPosY = 0.3f;
    private const int placePointIncrease = 3;

    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject[] counter;

    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject[] counterPrefab;
    [SerializeField] private GameObject placePointPrefab;

    [SerializeField] private string[] counterPartName = new string[] 
    {
        "Top", "Middle", "Bottom"
    };
    [SerializeField] private int counterPartCounter;

    [SerializeField] private int existingFloorParts;
    [SerializeField] private int existingCounterParts;
    [SerializeField] private int existingPlacePoints;

    public void ExpandFloor()
    {
        int previousFloorPos;
        
        GameObject floorPart;
        GameObject previousFloorPart;

        Vector2 floorPartPos;

        previousFloorPos = existingFloorParts;

        previousFloorPart = GameObject.Find($"Floor Part {previousFloorPos}");

        floorPartPos = new Vector2(
            previousFloorPart.transform.position.x + floorPosX,
            previousFloorPart.transform.position.y + floorPosY);
        
        floorPart = Instantiate(
            floorPrefab, floorPartPos, Quaternion.identity);

        floorPart.name = $"Floor Part {existingFloorParts + 1}";
        floorPart.transform.parent = grid.transform;

        existingFloorParts++;
    }

    public void ExpandCounter()
    {
        int previousCounterPos;

        GameObject[] counterParts = new GameObject[counterPartCounter];;
        GameObject[] previousCounterParts = new GameObject[counterPartCounter];

        Vector2[] counterPartPos = new Vector2[counterPartCounter];

        previousCounterPos = existingCounterParts;

        for (int i = 0; i < counterPartCounter; i++)
        {
            previousCounterParts[i] = GameObject.Find(
                $"{counterPartName[i]} Part {previousCounterPos}");

            Vector3 previousObjectPosition = 
                previousCounterParts[i].transform.position;

            counterPartPos[i] = new Vector2(
                previousObjectPosition.x - counterPosX * 20,
                previousObjectPosition.y - counterPosY * 20);

            if (i != 2)
                counterParts[i] = Instantiate(
                    counterPrefab[i], counterPartPos[i], 
                    transform.rotation, counter[i].transform);
            else
                counterParts[i] = Instantiate(
                    counterPrefab[i], counterPartPos[i], 
                    Quaternion.Euler(0, 0, 26.5f), counter[i].transform);

            counterParts[i].name = 
                $"{counterPartName[i]} Part {existingCounterParts + 1}";
        }

         existingCounterParts++;

         AddPlacementPoints(placePointIncrease);
        balcony.IncreasePositions(placePointIncrease);
    }

    private void AddPlacementPoints(int amount)
    {
        int previousPointPos;

        GameObject placePoint;
        GameObject previousPlacePoint;

        Vector2 placePointPos;

        previousPointPos = existingPlacePoints;

        previousPlacePoint = 
            GameObject.Find($"Placement Point {previousPointPos}");

        placePointPos = new Vector2(
            previousPlacePoint.transform.position.x - placePointPosX * 2,
            previousPlacePoint.transform.position.y - placePointPosY * 2);

        placePoint = Instantiate(placePointPrefab, placePointPos, 
            Quaternion.Euler(0, 0, 90), counter[3].transform);

        placePoint.name = $"Placement Point {existingPlacePoints + 1}";

        existingPlacePoints++;

        if (amount > 1) AddPlacementPoints(amount - 1);
    }
}
