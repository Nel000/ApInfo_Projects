using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    private bool inRange;
    public bool InRange => inRange;

    private Vector2 obstacle;
    public Vector2 Obstacle => obstacle;

    private GameObject obstaclePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange) obstacle = obstaclePos.transform.position;
        else obstacle = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("RANGE");
            inRange = true;
            obstaclePos = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 3)
        {
            Debug.Log("RANGE EXIT");
            inRange = false;
        }
    }
}
