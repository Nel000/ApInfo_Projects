using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int availableSeats = 4;
    [SerializeField] private int totalCustomers = 0;

    public int AvailableSeats
    { 
        get { return availableSeats; }
        set { availableSeats = value; }
    }

    [SerializeField] private List<GameObject> customers = new List<GameObject>();
    public List<GameObject> Customers { get { return customers; } }

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

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        waitLine = 0;

        //customers.Add(GameObject.Find("Customer 1"));
        StartCoroutine(CreateCustomer());
    }

    // Update is called once per frame
    void Update()
    {
        
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
            StartCoroutine(CreateCustomer());
        }
    }
}
