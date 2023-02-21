using UnityEngine;

public class Table : MonoBehaviour
{
    private Waiter waiterScr;

    [SerializeField] private bool isEmpty;
    public bool IsEmpty { get { return isEmpty; } set {isEmpty = value; } }

    public bool Clicked;

    [SerializeField] private GameObject servePos;

    // Start is called before the first frame update
    private void Start()
    {
        isEmpty = true;

        waiterScr = FindObjectOfType<Waiter>();
    }

    private void Update()
    {
        if (Clicked) ClickTable();
    }

    private void ClickTable()
    {
        Debug.Log("Clicked table");

        Clicked = false;

        if (!waiterScr.IsMoving 
            && waiterScr.CurrentTable != gameObject.name
            && !FindObjectOfType<GameManager>().IsLocked)
        {
            waiterScr.UpdateTables(waiterScr.CurrentTable, gameObject.name);

            StartCoroutine(waiterScr.Move(servePos.transform.position));

            /*if (!multiplePoints)
                StartCoroutine(waiterScr.Move(servePos.transform.position));
            else
            {
                if (waiterScr.PreviousTable == "Table 2"
                    && waiterScr.CurrentTable == "Table 4"
                    ||waiterScr.PreviousTable == "Table 4"
                    && waiterScr.CurrentTable == "Table 2"
                    || waiterScr.PreviousTable == "Center"
                    && waiterScr.CurrentTable == "Table 4"
                    || waiterScr.PreviousTable == "Balcony"
                    && waiterScr.CurrentTable == "Table 1"
                    || waiterScr.PreviousTable == "Balcony"
                    && waiterScr.CurrentTable == "Table 4")
                {
                    StartCoroutine(waiterScr.Move(servePos.transform.position,
                        true, secPos.transform.position));
                }
                else
                    StartCoroutine(waiterScr.Move(secPos.transform.position));
            }*/
        }

        if (waiterScr.IsOnTable &&
            waiterScr.CurrentTable == gameObject.name)
        {
            waiterScr.CheckCurrentTable();
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
