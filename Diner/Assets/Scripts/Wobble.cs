using UnityEngine;

public class Wobble : MonoBehaviour
{
    private Waiter waiter;
    private Customer customer;

    [SerializeField] private float speed, maxSpeed, updateValue, factor;

    [SerializeField] private bool isWaiter;

    private void Start()
    {
        if (isWaiter)
            waiter = GetComponent<Waiter>();
        else
            customer = GetComponentInParent<Customer>();
    }

    private void Update()
    {
        if (isWaiter && waiter.IsMoving
            || !isWaiter && customer.IsMoving)
        {
            speed += updateValue * Time.deltaTime;

            if (speed > maxSpeed)
            {
                speed = maxSpeed;
                updateValue = -updateValue;

                transform.eulerAngles = new Vector3(0, 0, factor);
            }

            if (speed < -maxSpeed)
            {
                speed = -maxSpeed;
                updateValue = -updateValue;

                transform.eulerAngles = new Vector3(0, 0, -factor);

            }
        }

        if (isWaiter && !waiter.IsMoving
            || !isWaiter && !customer.IsMoving)
        {
            speed = 0;
            transform.eulerAngles = Vector3.zero;
        } 
    }
}
