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

    [SerializeField] private GameObject customer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CreateCustomer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator CreateCustomer()
    {
        yield return new WaitForSeconds(5.0f);
        if (availableSeats > 0)
        {
            Instantiate(customer);
            StartCoroutine(CreateCustomer());
        }
    }
}
