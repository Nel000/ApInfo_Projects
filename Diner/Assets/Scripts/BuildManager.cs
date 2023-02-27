using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    private const float floorPosX = -5.5f, floorPosY = -2.75f;

    [SerializeField] private GameObject grid;

    [SerializeField] private GameObject floorPrefab;

    [SerializeField] private int existingFloorParts;

    private void Start()
    {

    }

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
}
