using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private float speed = 5.0f;

    private Vector2 target;

    [SerializeField] private GameObject[] tables;
    [SerializeField] private GameObject[] seats;

    private Vector2[] targets;

    [SerializeField] private Vector2 waitWall;

    [SerializeField] private int tableNum = 4;
    private int targetNum;
    
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {   
        targetNum = 0;
        canMove = true;

        StartCoroutine(Move(waitWall));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Waiting...");

        CheckTables();
    }

    private void CheckTables()
    {
        for (int i = 0; i < tableNum; i++)
        {
            if (tables[i].GetComponent<Table>().IsEmpty)
                targetNum++;
        }

        targets = new Vector2[targetNum];

        for (int j = 0; j < targetNum; j++)
        {
            targets[j] = seats[j].transform.position;
        }
        
        if (targetNum > 0)
            StartCoroutine(Move(targets[0]));
    }

    private IEnumerator Move(Vector2 target)
    {
        yield return new WaitForSeconds(0.5f);
        do
        {
            transform.position = Vector2.MoveTowards(
                transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        while (Vector2.Distance(transform.position, target) > 0.1f);
    }
}
