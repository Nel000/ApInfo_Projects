using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private bool isEmpty;
    public bool IsEmpty { get { return isEmpty; } set {isEmpty = value; } }

    [SerializeField] private GameObject waiter;
    [SerializeField] private GameObject servePos;

    // Start is called before the first frame update
    void Start()
    {
        isEmpty = true;
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked table");

        if (!waiter.GetComponent<Waiter>().IsMoving && !isEmpty 
            && !FindObjectOfType<GameManager>().IsLocked)
        {
            waiter.GetComponent<Waiter>().CurrentTable = gameObject.name;
            StartCoroutine(waiter.GetComponent<Waiter>().Move(
                servePos.transform.position));
        }       
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Customer>() 
            && !other.GetComponent<Customer>().IsLeaving)
        {
            Debug.Log($"Customer is on {name}");
            isEmpty = false;
        }    
        else if (other.GetComponent<Waiter>())
        {
            Debug.Log($"Waiter is on {name}");
        }
    }
}
