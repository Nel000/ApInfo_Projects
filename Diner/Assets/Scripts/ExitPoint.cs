using UnityEngine;

public class ExitPoint : MonoBehaviour
{
    private GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        gm.Customers.Remove(other.gameObject);
        Destroy(other.gameObject);
    }
}
