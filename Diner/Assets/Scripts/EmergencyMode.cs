using System.Collections;
using UnityEngine;

public class EmergencyMode : MonoBehaviour
{
    private GameManager gm;

    [SerializeField] private int customerLimit, emergencyScore;

    [SerializeField] private bool active;
    public bool Active => active;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void ActivateEmergency()
    {
        active = true;

        customerLimit = gm.Customers.Count / 2;
    }

    public void Progress(int value)
    {
        if (value > 0)
        {
            emergencyScore++;

            if (emergencyScore >= customerLimit) StartCoroutine(EndEmergency());
        }
        else
        {
            emergencyScore--;

            if (emergencyScore < 0) gm.EndGame();
        }
    }

    private IEnumerator EndEmergency()
    {
        yield return new WaitForEndOfFrame();
        active = false;
        emergencyScore = 0;
        gm.ResetCriticProbability(false);
    }
}
