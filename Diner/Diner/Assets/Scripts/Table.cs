using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] private bool isEmpty;
    public bool IsEmpty { get { return isEmpty; } }

    [SerializeField] private GameObject waiter;
    [SerializeField] private GameObject servePos;

    // Start is called before the first frame update
    void Start()
    {
        isEmpty = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Clicked table");

        StartCoroutine(
            waiter.GetComponent<Waiter>().Move(servePos.transform.position));
    }
}
