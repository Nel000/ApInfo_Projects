using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    private float speed = 10.0f;

    [SerializeField] private bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    [SerializeField] private string currentTable;

    // Start is called before the first frame update
    void Start()
    {
        currentTable = "";
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Table>())
        {
            currentTable = other.name;
            CheckCurrentTable();
        }
    }

    private void CheckCurrentTable()
    {
        foreach (GameObject customer in FindObjectOfType<GameManager>().Customers)
        {
            if (currentTable == customer.GetComponent<Customer>().Table
                && !customer.GetComponent<Customer>().IsAttended)
            {
                // Attend customer
                Debug.Log(
                    $"Waiter is attending {customer.name} at {currentTable}");
                customer.GetComponent<Customer>().MakeRequest();
            }
        }
    }

    public IEnumerator Move(Vector2 target)
    {
        Debug.Log("Waiter moving...");

        currentTable = "";
        isMoving = true;

        do
        {
            transform.position = Vector2.MoveTowards(
                transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        while (Vector2.Distance(transform.position, target) > 0.1f);

        isMoving = false;
    }
}
