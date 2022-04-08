using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private float speed = 5.0f;

    private Vector2 target;

    [SerializeField] private GameObject[] tables;
    [SerializeField] private GameObject[] seats;

    [SerializeField] private Vector2[] targets;

    [SerializeField] private Vector2[] waitWalls;

    [SerializeField] private int tableNum = 4;
    private int targetNum;
    private int targetDiff;
    
    [SerializeField] private bool canMove;
    [SerializeField] private bool hasChecked;

    // Start is called before the first frame update
    void Start()
    {   
        targetNum = 0;
        targetDiff = 0;
        canMove = true;

        for (int i = 0; i < tableNum; i++)
        {
            tables[i] = GameObject.Find($"Table {i + 1}");
            seats[i] = GameObject.Find($"Seat {i + 1}");
        }

        StartCoroutine(Move(waitWalls[1]));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Waiting...");

        if (other.name == "WaitWallSec")
        {
            Debug.Log("WaitWall sec");

            if (FindObjectOfType<GameManager>().WaitLine == 0)
                StartCoroutine(Move(waitWalls[0]));
            else
                FindObjectOfType<GameManager>().WaitLine++;
        }
        else if (other.name == "WaitWall")
        {
            Debug.Log("WaitWall main");

            //if(!hasChecked)
            CheckTables();
        }
    }

    private void CheckTables()
    {
        for (int i = 0; i < tableNum; i++)
        {
            if (tables[i].GetComponent<Table>().IsEmpty)
                targetNum++;
            else
                targetDiff++;
        }

        targets = new Vector2[targetNum];

        for (int j = 0; j < tableNum - targetDiff; j++)
        {
            targets[j] = seats[j + targetDiff].transform.position;
        }
        
        if (targetNum > 0)
            StartCoroutine(Move(targets[0]));
        else
        {
            FindObjectOfType<GameManager>().WaitLine++;
            //CheckTables();
        }

        //hasChecked = true;
    }

    private IEnumerator Move(Vector2 target)
    {
        yield return new WaitForSeconds(0.2f);
        do
        {
            transform.position = Vector2.MoveTowards(
                transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        while (Vector2.Distance(transform.position, target) > 0.1f);
    }
}
