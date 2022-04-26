using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int maxTime = 500;
    [SerializeField] private int currentTime;

    [SerializeField] private int availableSeats = 4;
    [SerializeField] private int totalCustomers = 0;

    public int AvailableSeats
    { 
        get { return availableSeats; }
        set { availableSeats = value; }
    }

    [SerializeField] private List<GameObject> customers = new List<GameObject>();
    public List<GameObject> Customers { get { return customers; } }

    [SerializeField] private Text timeValue;

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
        currentTime = -1;
        rand = new System.Random();
        waitLine = 0;

        StartCoroutine(RaiseTime());
        StartCoroutine(CreateCustomer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator RaiseTime()
    {
        do
        {
            currentTime++;
            timeValue.text = (currentTime * Time.timeScale).ToString();
            yield return new WaitForSecondsRealtime(1.0f);
        }
        while (currentTime < maxTime);

        if (currentTime >= maxTime)
        {
            Debug.Log("Time's up!");
            currentTime = maxTime;
        }
    }

    private IEnumerator CreateCustomer()
    {
        yield return new WaitForSeconds(rand.Next(5, 10));
        if (WaitLine < 2)
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
}
