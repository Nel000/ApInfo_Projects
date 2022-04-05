using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    private float speed = 10.0f;

    private bool isMoving;
    public bool IsMoving { get { return isMoving; } }

    // Start is called before the first frame update
    void Start()
    {
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Move(Vector2 target)
    {
        Debug.Log("Waiter moving...");

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
