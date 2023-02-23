using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int diffIncreaseTime = 30;

    public Waiter Waiter;
    public Balcony Balcony;

    [SerializeField] private CameraCtrl camCtrl;

    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private GameObject endCanvas;

    [SerializeField] private int maxTime = 500;
    [SerializeField] private int currentTime;
    [SerializeField] private int currentScore;

    [SerializeField] private int availableSeats = 4;
    [SerializeField] private int totalCustomers = 0;

    public bool IsLocked { get; set; }

    private bool inEndGame;

    public int AvailableSeats
    { 
        get { return availableSeats; }
        set { availableSeats = value; }
    }

    [SerializeField] private List<GameObject> customers = new List<GameObject>();
    public List<GameObject> Customers { get { return customers; } }

    [SerializeField] private Text timeValue;
    [SerializeField] private Text scoreValue;
    [SerializeField] private Text endScoreValue;

    private System.Random rand;

    [SerializeField] private int waitLine;
    public int WaitLine 
    { 
        get 
        { 
            return waitLine; 
        } 
        set
        {
            if (waitLine <= 2)
                waitLine = value;
        }
    }

    [SerializeField] private GameObject customer;

    [SerializeField] private GameObject[] meals;
    public GameObject[] Meals { get { return meals; } }

    // Start is called before the first frame update
    void Start()
    {
        Waiter = FindObjectOfType<Waiter>();
        Balcony = FindObjectOfType<Balcony>();
        camCtrl = FindObjectOfType<CameraCtrl>();

        currentTime = -1;
        currentScore = 0;
        rand = new System.Random();
        waitLine = 0;

        StartCoroutine(RaiseTime());
        StartCoroutine(CreateCustomer());
    }

    private void OnMouseDown()
    {
        Debug.Log("Back to start");

        if (!Waiter.GetComponent<Waiter>().IsMoving && !IsLocked
            && Waiter.CurrentTable != "Center")
        {
            Waiter.UpdateTables(Waiter.CurrentTable, "Center");
            Waiter.Center();

            StartCoroutine(Waiter.GetComponent<Waiter>().Move(transform.position));
        }
    }

    private IEnumerator RaiseTime()
    {
        int updateTime = 0;

        do
        {
            if (updateTime >= diffIncreaseTime)
            {
                BuildTable(2);
                StartCoroutine(camCtrl.IncreaseSize());
                updateTime = 0;
            }

            currentTime++;
            updateTime++;
            timeValue.text = (currentTime * Time.timeScale).ToString();
            yield return new WaitForSecondsRealtime(1.0f);
        }
        while (currentTime < maxTime);

        if (currentTime >= maxTime)
        {
            Debug.Log("Time's up!");
            currentTime = maxTime;
            timeValue.text = (currentTime * Time.timeScale).ToString();
            inEndGame = true;
            EndGame();
        }
    }

    private void BuildTable(int num)
    {
        GameObject table;

        for (int i = 0; i < num; i++)
        {
            table = GameObject.Find($"Table {availableSeats + 1}");
            table.GetComponent<ObjectBuilder>().StartBuild();
            availableSeats++;
        }
    }

    public void OfficialTable()
    {
        foreach(GameObject customer in customers)
            customer.GetComponent<Customer>().UpdateTables();
    }

    public void UpdateScore(int value)
    {
        currentScore += value;
        scoreValue.text = currentScore.ToString();
    }

    private IEnumerator CreateCustomer()
    {
        yield return new WaitForSeconds(rand.Next(5, 10));
        if (WaitLine < 2 && !inEndGame)
        {
            totalCustomers++;

            GameObject currentCustomer = Instantiate(customer);
            currentCustomer.name = $"Customer {totalCustomers}";
            customers.Add(currentCustomer);
        }

        StartCoroutine(CreateCustomer());
    }

    public GameObject DefineMeal()
    {
        return meals[rand.Next(0, meals.Length)];
    }

    private void EndGame()
    {
        if (customers.Count <= 0)
        {
            IsLocked = true;
            mainCanvas.SetActive(false);
            endCanvas.SetActive(true);
            endScoreValue.text = $"Score: {currentScore}";
        }
        else
            Invoke("EndGame", 1.0f);
    }

    public void ButtonBehaviour()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
