using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const float lineSpotPosX = 1.8f, lineSpotPosY = 1.35f;

    public Waiter Waiter;
    public Balcony Balcony;

    [SerializeField] private Score scoreMng;
    [SerializeField] private BuildManager bm;
    [SerializeField] private CameraCtrl camCtrl;

    [SerializeField] private GameObject lineSpotPrefab;

    [SerializeField] private int criticProbability;
    public int CriticProbability => criticProbability;

    [SerializeField] private int availableSeats = 4;
    public int AvailableSeats
    { 
        get { return availableSeats; }
        set { availableSeats = value; }
    }
    [SerializeField] private int totalCustomers = 0;
    public int TotalCustomers => totalCustomers;
    [SerializeField] private int totalLineSpots = 1;
    public int TotalLineSpots => totalLineSpots;

    public bool IsLocked { get; set; }

    private bool inEndGame;
    public bool InEndGame => inEndGame;

    [SerializeField] private bool hasCritic;
    public bool HasCritic => hasCritic;

    [SerializeField] private List<GameObject> customers = new List<GameObject>();
    public List<GameObject> Customers { get { return customers; } }

    private List<LineSpot> lineSpotList = new List<LineSpot>();
    public List<LineSpot> LineSpotList => lineSpotList;

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
            if (waitLine <= totalLineSpots)
                waitLine = value;
        }
    }

    [SerializeField] private GameObject[] meals;
    public GameObject[] Meals { get { return meals; } }

    private void Start()
    {
        scoreMng = FindObjectOfType<Score>();
        Waiter = FindObjectOfType<Waiter>();
        Balcony = FindObjectOfType<Balcony>();
        bm = GetComponent<BuildManager>();
        camCtrl = FindObjectOfType<CameraCtrl>();

        rand = new System.Random();
        waitLine = 0;

        lineSpotList.Add(
            GameObject.Find("Line Spot 1").GetComponent<LineSpot>());

        lineSpotList.Add(
            GameObject.Find("Line Spot 2").GetComponent<LineSpot>());
    }

    private void OnMouseUpAsButton()
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

    public void ExpandSpace()
    {
        ExpandLine();

        bm.BuildTable(2);
        availableSeats = bm.AvailableSeats;
        bm.ExpandFloor();
        bm.ExpandCounter();

        //StartCoroutine(camCtrl.IncreaseSize());
        camCtrl.UpdateSpace();
    }

    private void ExpandLine()
    {
        GameObject lineSpot;

        int previousPosition;

        Vector2 spotPosition;
        GameObject previousSpot;

        previousPosition = totalLineSpots;

        previousSpot = GameObject.Find($"Line Spot {previousPosition}");

        spotPosition = new Vector2(
            previousSpot.transform.position.x - lineSpotPosX,
            previousSpot.transform.position.y - lineSpotPosY);
        
        lineSpot = Instantiate(
            lineSpotPrefab, spotPosition, Quaternion.identity);

        lineSpotList.Add(lineSpot.GetComponent<LineSpot>());
        totalLineSpots++;
    }

    public void OfficialTable()
    {
        foreach(GameObject customer in customers)
            customer.GetComponent<Customer>().UpdateTables();
    }

    public void UpdateScore(int value)
    {
        if (!hasCritic) criticProbability += 5;
        scoreMng.ProcessScore(value);
    }

    public void RemoveCritic() => hasCritic = false;

    public void ResetCriticProbability()
    {
        hasCritic = true;
        criticProbability = 1;
    }

    public void AddCustomer(GameObject customer)
    {
        totalCustomers++;
        customers.Add(customer);
    }

    public GameObject DefineMeal() => meals[rand.Next(0, meals.Length)];

    public void EndGame()
    {
        inEndGame = true;

        if (customers.Count <= 0)
        {
            IsLocked = true;
            scoreMng.EndScore();
        }
        else
            Invoke("EndGame", 1.0f);
    }

    public void ButtonBehavior()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
