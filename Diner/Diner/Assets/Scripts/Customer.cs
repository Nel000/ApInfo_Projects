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

    [SerializeField] private Vector2[] movePoints;

    [SerializeField] private int tableNum = 4;
    private int targetNum;
    private int targetDiff;

    [SerializeField] private string table;
    public string Table { get {return table; } }
    
    [SerializeField] private bool clearedToMove;
    [SerializeField] private bool goingToSeat;
    public bool GoingToSeat { get { return goingToSeat; } }

    [SerializeField] private bool canMove;
    [SerializeField] private bool hasChecked;
    [SerializeField] private bool isWaiting;
    [SerializeField] private bool isAttended;
    public bool IsAttended { get { return isAttended; } }

    public bool IsLeaving { get; private set; }

    [SerializeField] private bool isServed;

    [SerializeField] private int waitTime;
    [SerializeField] private int maxTime = 50; 

    // Start is called before the first frame update
    void Start()
    {   
        clearedToMove = true;
        goingToSeat = false;
        table = "";
        targetNum = 0;
        targetDiff = 0;
        canMove = true;

        for (int i = 0; i < tableNum; i++)
        {
            tables[i] = GameObject.Find($"Table {i + 1}");
            seats[i] = GameObject.Find($"Seat {i + 1}");
        }

        StartCoroutine(Move(movePoints[1]));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "WaitWallSec")
        {
            Debug.Log("WaitWall sec");

            if (FindObjectOfType<GameManager>().WaitLine == 0)
                StartCoroutine(Move(movePoints[0]));
            else
            {
                Debug.Log("Waiting...");

                FindObjectOfType<GameManager>().WaitLine++;
                StartCoroutine(LineWait());
            }    
        }
        else if (other.name == "WaitWall")
        {
            Debug.Log("WaitWall main");

            //if(!hasChecked)
            StartCoroutine(CheckTables());
        }
        else if (other.GetComponent<Table>())
        {
            table = other.name;
            goingToSeat = false;
            isWaiting = true;
            StartCoroutine(Wait());
        }
    }
    
    private IEnumerator LineWait()
    {
        Debug.Log("Waiting in line");
        yield return new WaitForSeconds(1.0f);
        if (FindObjectOfType<GameManager>().WaitLine == 1)
            StartCoroutine(Move(movePoints[0]));
        else
            StartCoroutine(LineWait());
    }

    private IEnumerator CheckTables()
    {
        bool foundEmptyTable = false;

        /*for (int i = 0; i < tableNum; i++)
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
            Debug.Log("Waiting...");
            FindObjectOfType<GameManager>().WaitLine++;
            //CheckTables();
        }*/

        clearedToMove = true;

        foreach(GameObject oC in FindObjectOfType<GameManager>().Customers)
        {
            if (oC.GetComponent<Customer>().GoingToSeat)
            {
                clearedToMove = false;
            }
        }

        foreach (GameObject table in tables)
        {
            if (table.GetComponent<Table>().IsEmpty && !foundEmptyTable
                && clearedToMove)
            {
                foundEmptyTable = true;

                if(FindObjectOfType<GameManager>().WaitLine > 0)
                    FindObjectOfType<GameManager>().WaitLine--;

                goingToSeat = true;
                StartCoroutine(Move(
                    GameObject.Find($"Seat {table.name}").transform.position));
            }
        }

        if (!foundEmptyTable)
        {
            Debug.Log("Waiting...");

            if (FindObjectOfType<GameManager>().WaitLine < 1)
                FindObjectOfType<GameManager>().WaitLine++;

            yield return new WaitForSeconds(1.0f);
            StartCoroutine(CheckTables());
        }

        //hasChecked = true;
    }

    public void MakeRequest()
    {
        isAttended = true;
        waitTime = 0;
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
    
    private IEnumerator Wait()
    {
        do
        {
            waitTime++;
            yield return new WaitForSeconds(1.0f);
        }
        while (waitTime < maxTime && !isServed);

        if (isServed)
        {
            // Eat and leave after some time
        }
        else if (waitTime >= maxTime && !isServed)
        {
            // Leave immediately
            IsLeaving = true;

            Debug.Log($"{name} heads out");
            StartCoroutine(Move(movePoints[2]));
            FindObjectOfType<GameManager>().Customers.Remove(gameObject);
            GameObject.Find(table).GetComponent<Table>().IsEmpty = true;
            Destroy(gameObject, 2.0f);
        }
    }
}
