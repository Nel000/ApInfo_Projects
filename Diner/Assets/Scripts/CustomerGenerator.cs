using System.Collections;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{
    // Inc: 0.3, 0.225
    private const float updatePosX = 5.7f, updatePosY = 4.275f;

    private GameManager gm;
    private EmergencyMode emergency;

    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private GameObject criticPrefab;

    private int lineSpots;

    private System.Random rand;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        emergency = FindObjectOfType<EmergencyMode>();

        rand = new System.Random();

        lineSpots = gm.TotalLineSpots;

        StartCoroutine(CreateCustomer(customerPrefab));
    }

    private IEnumerator CreateCustomer(GameObject customer)
    {
        yield return new WaitForSeconds(rand.Next(5, 10));

        if (lineSpots < gm.TotalLineSpots) UpdatePosition();

        if (!emergency.Active && !gm.InEndGame)
        {
            if (gm.WaitLine < gm.TotalLineSpots)
            {
                GameObject currentCustomer = Instantiate(customer,
                    transform.position, Quaternion.identity);
                currentCustomer.name = $"Customer {gm.TotalCustomers}";
                gm.AddCustomer(currentCustomer);
            }

            int prob = rand.Next(0, 100);

            if (prob <= gm.CriticProbability && !gm.HasCritic)
            {
                StartCoroutine(CreateCustomer(criticPrefab));
                gm.ResetCriticProbability();
            }
            else StartCoroutine(CreateCustomer(customerPrefab));
        }
        else StartCoroutine(CreateCustomer(customerPrefab));
    }

    private void UpdatePosition()
    {
        lineSpots = gm.TotalLineSpots;

        transform.position = new Vector2(
            transform.position.x - updatePosX,
            transform.position.y - updatePosY);
    }
}
