using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Customer : MonoBehaviour
{
    private GameManager gm;

    [SerializeField] private Canvas customerCanvas;
    [SerializeField] private Image statMeter;

    private float speed = 5.0f;

    [SerializeField] private int defaultScore = 10;

    private Vector2 target;

    [SerializeField] private GameObject mealBalloon;

    [SerializeField] private GameObject[] tables;
    [SerializeField] private GameObject[] seats;

    [SerializeField] private Vector2[] targets;

    [SerializeField] private Vector2[] movePoints;

    [SerializeField] private int tableNum = 4;

    [SerializeField] private string table;
    public string Table { get {return table; } }
    
    [SerializeField] private bool clearedToMove;
    [SerializeField] private bool goingToSeat;
    public bool GoingToSeat { get { return goingToSeat; } }

    [SerializeField] private bool hasChecked;
    [SerializeField] private bool isAttended;
    public bool IsAttended { get { return isAttended; } }

    public bool IsLeaving { get; private set; }

    [SerializeField] private bool isServed;
    [SerializeField] private bool waitReset;

    [SerializeField] private int waitTime;
    [SerializeField] private int maxTime = 50; 

    [SerializeField] private GameObject meal;
    [SerializeField] private GameObject mealImg;
    [SerializeField] private GameObject[] stateImg;

    void Start()
    {   
        gm = FindObjectOfType<GameManager>();

        customerCanvas.worldCamera = 
            GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        statMeter.color = Color.green;

        clearedToMove = true;
        goingToSeat = false;
        table = "";

        for (int i = 0; i < tableNum; i++)
        {
            tables[i] = GameObject.Find($"Table {i + 1}");
            seats[i] = GameObject.Find($"Seat {i + 1}");
        }

        meal = gm.DefineMeal();

        StartCoroutine(Move(movePoints[1]));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "WaitWallSec")
        {
            Debug.Log("WaitWall sec");

            if (gm.WaitLine == 0)
                StartCoroutine(Move(movePoints[0]));
            else
            {
                Debug.Log("Waiting...");

                gm.WaitLine++;
                StartCoroutine(StatUpdate(maxTime / 2));
                StartCoroutine(LineWait(0));
            }    
        }
        else if (other.name == "WaitWall")
        {
            Debug.Log("WaitWall main");

            StartCoroutine(StatUpdate(maxTime / 2));
            StartCoroutine(CheckTables(0));
        }
        else if (other.GetComponent<Table>() && !IsLeaving)
        {
            table = other.name;
            goingToSeat = false;
            StartCoroutine(Wait());
            StartCoroutine(StatUpdate(maxTime / 1.5f));
            if (gm.Waiter.CurrentTable == table)
                StartCoroutine(MakeRequest());
        }
    }
    
    private IEnumerator LineWait(int waitTime)
    {
        Debug.Log("Waiting in line");
        yield return new WaitForSeconds(1.0f);
        if (gm.WaitLine == 1)
            StartCoroutine(Move(movePoints[0]));
        else
            if (waitTime >= maxTime / 2)
            {
                gm.UpdateScore(-defaultScore);
                gm.WaitLine--;
                mealBalloon.SetActive(true);
                stateImg[0].SetActive(true);
                Leave();
            }
            else
                StartCoroutine(LineWait(waitTime + 1));
    }

    private IEnumerator CheckTables(int waitTime)
    {
        bool foundEmptyTable = false;

        clearedToMove = true;

        foreach(GameObject oC in gm.Customers)
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
                ResetStat();

                if(gm.WaitLine > 0)
                    gm.WaitLine--;

                goingToSeat = true;
                StartCoroutine(Move(
                    GameObject.Find($"Seat {table.name}").transform.position));
            }
        }

        if (!foundEmptyTable)
        {
            if (waitTime >= maxTime / 2)
            {
                gm.UpdateScore(-defaultScore);
                gm.WaitLine--;
                mealBalloon.SetActive(true);
                stateImg[0].SetActive(true);
                Leave();
            }
            else
            {
                Debug.Log("Waiting...");

                if (gm.WaitLine < 1)
                    gm.WaitLine++;

                yield return new WaitForSeconds(1.0f);
                StartCoroutine(CheckTables(waitTime + 1));
            }
        }
    }

    public IEnumerator MakeRequest()
    {
        Debug.Log("Customer is being attended.");

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
            StartCoroutine(Eat());
        }
        // Else, leave immediately
        else
        {
            gm.UpdateScore(defaultScore * -2);
            GameObject.Find(table).GetComponent<Table>().IsEmpty = true;
            Destroy(mealImg);
            stateImg[0].SetActive(true);
            Invoke("Leave", 0.1f);
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
            gm.UpdateScore(-defaultScore);
            Leave();
        }
    }

    private void Leave()
    {
        // Leave immediately
        IsLeaving = true;

        Debug.Log($"{name} heads out");
        StartCoroutine(Move(movePoints[2]));
        gm.Customers.Remove(gameObject);
        Destroy(gameObject, 3.0f);
    }
}
