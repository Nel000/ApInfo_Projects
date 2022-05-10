using System.Collections;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    [SerializeField] private InventorySlot inventorySlot;
    public InventorySlot InventorySlot { get => inventorySlot; }

    private float speed = 10.0f;

    [SerializeField] private bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    [SerializeField] private bool isOnTable;
    public bool IsOnTable => isOnTable;

    [SerializeField] private string currentTable;
    public string CurrentTable 
    { 
        get => currentTable; 
        set => currentTable = value;
    }

    [SerializeField] private bool hasMeal;
    public bool HasMeal { get => hasMeal; }

    public int MealIndex { get; set; }

    void Start()
    {
        currentTable = "";
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Table>() && other.name == currentTable)
        {
            CheckCurrentTable();
            isOnTable = true;
        }
    }

    public void CheckCurrentTable()
    {
        foreach (GameObject customer in FindObjectOfType<GameManager>().Customers)
        {
            if (currentTable == customer.GetComponent<Customer>().Table)
            {
                if (!customer.GetComponent<Customer>().IsAttended)
                {
                    // Attend customer
                    Debug.Log(
                        $"Waiter is attending {customer.name} at {currentTable}");
                    customer.GetComponent<Customer>().MakeRequest();
                }
                else if (customer.GetComponent<Customer>().IsAttended && hasMeal)
                {
                    // Serve customer
                    Debug.Log(
                        $"Waiter served {customer.name} at {currentTable}");
                    customer.GetComponent<Customer>().GetServed();
                    RemoveFromInventory();
                }
            }
        }
    }

    public IEnumerator Move(Vector2 target)
    {
        Debug.Log("Waiter moving...");

        isMoving = true;
        isOnTable = false;

        do
        {
            transform.position = Vector2.MoveTowards(
                transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        while (Vector2.Distance(transform.position, target) > 0.1f);

        isMoving = false;
    }

    public void AddToInventory(int i)
    {
        Debug.Log($"Added meal #{i} to inventory.");
        MealIndex = i;
        hasMeal = true;
        inventorySlot.UpdateInventory(i);
    }

    public void RemoveFromInventory()
    {
        Debug.Log("Removed meal from inventory.");
        hasMeal = false;
        inventorySlot.UpdateInventory(MealIndex);
    }
}
