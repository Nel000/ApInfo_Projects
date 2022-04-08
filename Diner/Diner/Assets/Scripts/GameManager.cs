using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int availableSeats = 4;
    public int AvailableSeats
    { 
        get { return availableSeats; }
        set { availableSeats = value; }
    }

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
            if (waitLine < 2)
                waitLine = value;
        }
    }

    [SerializeField] private GameObject customer;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        waitLine = 0;

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
            Instantiate(customer);
            StartCoroutine(CreateCustomer());
        }
    }
}
