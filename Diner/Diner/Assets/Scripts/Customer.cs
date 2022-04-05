using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private float speed = 5.0f;

    private Vector2 target;

    [SerializeField] private GameObject[] tables;

    private Vector2[] targets;

    private int targetNum;
    
    private bool canMove;

    // Start is called before the first frame update
    void Start()
    {   
        targetNum = 0;
        canMove = true;

        CheckTables();

        StartCoroutine(Move(targets[0]));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckTables()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            if (tables[i].GetComponent<Table>().IsEmpty)
                targetNum++;
                
        }

        targets = new Vector2[targetNum];

        for (int j = 0; j < targetNum; j++)
        {
            targets[j] = tables[j].transform.position;
        }
    }

    private IEnumerator Move(Vector2 target)
    {
        yield return new WaitForSeconds(1.0f);
        do
        {
            transform.position = Vector2.MoveTowards(
                transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        while (Vector2.Distance(transform.position, target) > 0.1f);
    }
}
