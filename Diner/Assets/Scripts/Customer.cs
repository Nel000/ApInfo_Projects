using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    private GameManager gm;
    private ExitPoint exit;

    [SerializeField] private Canvas customerCanvas;
    [SerializeField] private Image statMeter;

    [SerializeField] private int defaultScore = 10;

    private Vector2 target;

    [SerializeField] private GameObject mealBalloon;

    [SerializeField] private GameObject[] tables;
    [SerializeField] private List<Table> tableList;

    [SerializeField] private GameObject linePos;

    [SerializeField] private Vector2[] movePoints;
    [SerializeField] private Vector2 nextLinePos;

    [SerializeField] private int tableNum = 4;

    [SerializeField] private string table;
    public string Table { get {return table; } }
    
    [SerializeField] private bool clearedToMove;
    [SerializeField] private bool goingToSeat;
    public bool GoingToSeat { get { return goingToSeat; } }

    [SerializeField] private bool isAttended;
    public bool IsAttended { get { return isAttended; } }

    public bool IsLeaving { get; private set; }

    [SerializeField] private bool isMoving;
    public bool IsMoving => isMoving;

    [SerializeField] private bool decided;
    public bool Decided => decided;

    [SerializeField] private bool enteredLine;
    [SerializeField] private bool isServed;
    [SerializeField] private bool waitReset;

    [SerializeField] private bool isCritic;
    public bool IsCritic => isCritic;

    [SerializeField] private int waitTime;
    [SerializeField] private int maxTime = 50; 

    [SerializeField] private GameObject meal;
    [SerializeField] private GameObject mealImg;
    [SerializeField] private GameObject[] stateImg;

    private NavMeshAgent agent;

    private void Awake()
    {   
        agent = GetComponent<NavMeshAgent>();
		agent.updateRotation = false;
		agent.updateUpAxis = false;

        gm = FindObjectOfType<GameManager>();
        exit = FindObjectOfType<ExitPoint>();

        customerCanvas.worldCamera = 
            GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        statMeter.color = Color.green;

        clearedToMove = true;
        goingToSeat = false;
        table = "";

        UpdateTables();

        meal = gm.DefineMeal();

        GetInLine();
    }

    private void GetInLine()
    {
        int lineLength = gm.TotalLineSpots;

        linePos = 
            gm.LineSpotList.ElementAt(lineLength - 1).gameObject;

        nextLinePos = new Vector2(
            linePos.transform.position.x, linePos.transform.position.y);

        StartCoroutine(Move(nextLinePos));
    }

    public void UpdateTables()
    {
        tableNum = gm.AvailableSeats;

        tables = new GameObject[tableNum];
        tableList = new List<Table>();

        for (int i = 0; i < tableNum; i++)
        {
            tables[i] = GameObject.Find($"Table {i + 1}");
            tableList.Add(tables[i].GetComponent<Table>());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<LineSpot>())
        {
            LineSpot currentSpot = other.GetComponent<LineSpot>();

            currentSpot.IsOccupied = true;

            if (other.name == "Line Spot 1")
            {
                Debug.Log("WaitWall main");

                isMoving = false;
                waitReset = false;
                StartCoroutine(StatUpdate(maxTime / 2));
                CheckTables(currentSpot, 0);
            }
            else
            {
                Debug.Log("WaitWall sec");

                if (!enteredLine)
                {
                    gm.WaitLine++;
                    enteredLine = true;
                }

                LineSpot nextSpot = gm.LineSpotList.ElementAt(
                    currentSpot.Position - 2);

                linePos = nextSpot.gameObject;
                
                nextLinePos = new Vector2(
                    linePos.transform.position.x, linePos.transform.position.y);

                if (gm.WaitLine < gm.TotalLineSpots
                    && nextSpot.Position >= gm.WaitLine)
                {
                    currentSpot.IsOccupied = false;
                    StartCoroutine(Move(nextLinePos));
                } 
                else
                {
                    Debug.Log("Waiting...");

                    isMoving = false;
                    StartCoroutine(StatUpdate(maxTime / 2));
                    StartCoroutine(LineWait(0, currentSpot, nextSpot));
                }
            }
        }
        else if (other.GetComponent<Table>() && !IsLeaving && goingToSeat)
        {
            if (other.name == table)
            {
                goingToSeat = false;
                StartCoroutine(Wait());
                StartCoroutine(StatUpdate(maxTime / 1.5f));
                if (gm.Waiter.CurrentTable == table 
                    && !gm.Waiter.IsMoving && decided)
                    StartCoroutine(MakeRequest());
            }
        }
    }
    
    private IEnumerator LineWait(int waitTime, LineSpot current, LineSpot next)
    {
        Debug.Log($"{name} waiting in line");

        yield return new WaitForSeconds(1.0f);
        if (!next.IsOccupied)
        {
            Debug.Log($"{name} waiting can move");
            ResetStat();
            current.IsOccupied = false;
            StartCoroutine(Move(nextLinePos));
        }
            
        else
        {
            if (waitTime >= maxTime / 2)
            {
                if (isCritic) gm.UpdateScore(-defaultScore * 5, true);
                gm.UpdateScore(-defaultScore);
                gm.WaitLine--;
                mealBalloon.SetActive(true);
                stateImg[0].SetActive(true);
                current.IsOccupied = false;
                Leave();
            }
            else
            {
                Debug.Log($"{name} waiting cannot move");
                StartCoroutine(LineWait(waitTime + 1, current, next));
            }
        }
    }

    private void CheckTables(
        LineSpot spot, int waitTime, bool addedToLine = false)
    {
        bool foundEmptyTable = false;

        clearedToMove = true;

        List<Table> clearTables = new List<Table>();

        Debug.Log($"{gameObject.name} Checking tables...");

        foreach(GameObject oC in gm.Customers)
        {
            if (oC.GetComponent<Customer>().GoingToSeat)
            {
                clearedToMove = false;
            }
        }

        foreach (Table table in tableList)
        {
            if (table.enabled == true)
            {
                if (table.IsEmpty && !foundEmptyTable
                && clearedToMove)
                {
                    Debug.Log($"{gameObject.name} add {table.name}");
                    clearTables.Add(table);
                }
            }
        }

        if (clearTables.Count > 0)
        {
            Table tableObj;

            Debug.Log($"{gameObject.name} found table...");

            if (clearTables.Count > 1)
                tableObj = DefineTable(clearTables);
            else
                tableObj = clearTables[0];

            foundEmptyTable = true;
             
            ResetStat();

            if(gm.WaitLine > 0)
                gm.WaitLine--;

            goingToSeat = true;

            table = tableObj.name;

            spot.IsOccupied = false;

            StartCoroutine(Move(
                GameObject.Find($"Seat {table}").transform.position));
        }

        if (!foundEmptyTable)
        {
            if (waitTime >= maxTime / 2)
            {
                if (isCritic) gm.UpdateScore(-defaultScore * 5, true);
                else gm.UpdateScore(-defaultScore);
                gm.WaitLine--;
                mealBalloon.SetActive(true);
                stateImg[0].SetActive(true);
                spot.IsOccupied = false;
                Leave();
            }
            else
            {
                Debug.Log("Waiting...");

                StartCoroutine(RecheckTables(waitTime, spot));
            }
        }
    }

    private IEnumerator RecheckTables(int waitTimeLocal, LineSpot spot)
    {
        yield return new WaitForSeconds(1.0f);
        CheckTables(spot, waitTimeLocal + 1, true);
    }

    private Table DefineTable(List<Table> tables)
    {
        System.Random rand = new System.Random();

        Table selectedTable = tables[rand.Next(0, tables.Count)];

        return selectedTable;
    }

    public IEnumerator MakeRequest()
    {
        Debug.Log("Customer is being attended.");

        yield return new WaitForEndOfFrame();
        isAttended = true;
        waitTime = 0;
        ResetStat();
        yield return new WaitForEndOfFrame();
        waitReset = false;
        StartCoroutine(StatUpdate(maxTime));

        mealBalloon.SetActive(true);
        mealImg = Instantiate(meal, mealBalloon.transform);
    }

    public void GetServed()
    {
        isServed = true;

        // If served meal equals requested meal
        if (gm.Waiter.InventorySlot.SlotImage.name == meal.name)
        {
            Destroy(mealImg);
            stateImg[2].SetActive(true);
            ResetStat();
            StartCoroutine(Eat());
        }
        // Else, leave immediately
        else
        {
            if (isCritic) gm.UpdateScore(defaultScore * -5, true);
            else gm.UpdateScore(defaultScore * -2);
            GameObject.Find(table).GetComponent<Table>().IsEmpty = true;
            Destroy(mealImg);
            stateImg[0].SetActive(true);
            ResetStat();
            statMeter.color = Color.red;
            Leave();
        }
    }

    private IEnumerator Eat()
    {
        int time = 0, eatTime = 10;
        do
        {
            Debug.Log("Eating...");
            time++;
            yield return new WaitForSeconds(1.0f);
        }
        while (time < eatTime);

        EvaluateMeal();
    }

    public void EvaluateMeal()
    {
        if (isCritic)
        {
            gm.UpdateScore(gm.Balcony.Meals[gm.Waiter.MealIndex].Score * 5, true);
            gm.RemoveCritic();
        }
        else
            gm.UpdateScore(gm.Balcony.Meals[gm.Waiter.MealIndex].Score * 2);
        GameObject.Find(table).GetComponent<Table>().IsEmpty = true;
        stateImg[2].SetActive(false);
        stateImg[1].SetActive(true);
        Leave();
    }

    private IEnumerator StatUpdate(float totalTime)
    {
        float time = 0;
        
        do
        {
            if (waitReset)
                yield break;

            if (statMeter.transform.localPosition.x >= 0.5
                && statMeter.transform.localPosition.x < 0.8)
                statMeter.color = Color.yellow;
            
            if (statMeter.transform.localPosition.x >= 0.8)
                statMeter.color = Color.red;

            statMeter.transform.localPosition = new Vector3(
                Mathf.Lerp(0, 1, time / totalTime), 0, 0);
            time += Time.deltaTime;
            yield return null;
        }
        while (time < totalTime && !waitReset);

        yield return new WaitForEndOfFrame();
        Debug.Log("Reset stat update");

        statMeter.transform.localPosition = new Vector3(0, 0, 0);
        waitReset = false;
    }

    private void ResetStat()
    {
        statMeter.transform.localPosition = new Vector3(0, 0, 0);
        statMeter.color = Color.green;
        waitReset = true;
    }

    private IEnumerator Move(Vector2 target)
    {
        yield return new WaitForSeconds(0.2f);
        isMoving = true;

        agent.SetDestination(target);
    }
    
    private IEnumerator Wait()
    {
        isMoving = false;

        decided = true; // Change later to own method

        do
        {
            waitTime++;
            yield return new WaitForSeconds(1.0f);
        }
        while (waitTime <= maxTime / 1.5 && !isAttended);

        if (isAttended)
        {
            do
            {
                waitTime++;
                yield return new WaitForSeconds(1.0f);
            }
            while (waitTime < maxTime && !isServed);
        }

        if (waitTime >= maxTime / 1.5 && !isAttended
            || waitTime >= maxTime && isAttended && !isServed)
        {
            // Leave immediately
            GameObject.Find(table).GetComponent<Table>().IsEmpty = true;
            if (mealImg != null)
                mealImg.SetActive(false);
            mealBalloon.SetActive(true);
            stateImg[0].SetActive(true);
            if (isCritic) gm.UpdateScore(-defaultScore * 5, true);
            else gm.UpdateScore(-defaultScore);
            Leave();
        }
    }

    private void Leave()
    {
        IsLeaving = true;

        Debug.Log($"{name} heads out");
        StartCoroutine(Move(exit.transform.position));
    }
}
