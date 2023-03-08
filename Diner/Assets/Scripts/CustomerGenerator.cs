using System.Collections;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{
    // Inc: 0.3, 0.225
    private const float updatePosX = 5.7f, updatePosY = 4.275f;

    private GameManager gm;

    [SerializeField] private GameObject customerPrefab;

    private int lineSpots;

    private System.Random rand;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();

        rand = new System.Random();

        lineSpots = gm.TotalLineSpots;

        StartCoroutine(CreateCustomer());
    }

    private IEnumerator CreateCustomer()
    {
        yield return new WaitForSeconds(rand.Next(5, 10));

        if (lineSpots < gm.TotalLineSpots) UpdatePosition();

        if (gm.WaitLine < gm.TotalLineSpots && !gm.InEndGame)
        {
            GameObject currentCustomer = Instantiate(customerPrefab,
                transform.position, Quaternion.identity);
            currentCustomer.name = $"Customer {gm.TotalCustomers}";
            gm.AddCustomer(currentCustomer);
        }

        StartCoroutine(CreateCustomer());
    }

    private void UpdatePosition()
    {
        lineSpots = gm.TotalLineSpots;

        transform.position = new Vector2(
            transform.position.x - updatePosX,
            transform.position.y - updatePosY);
    }
}
