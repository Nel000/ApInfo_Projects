using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private const int diffIncreaseTime = 60;

    private GameManager gm;
    private Rating rating;

    [SerializeField] private Text timeValue;

    [SerializeField] private int currentTime, maxTime;
    public int CurrentTime => currentTime;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        rating = FindObjectOfType<Rating>();

        currentTime = -1;

        StartCoroutine(RaiseTime());
    }

    private IEnumerator RaiseTime()
    {
        int updateTime = 0;

        while (currentTime < maxTime)
        {
            if (!rating.InEmergency)
            {
                if (updateTime >= diffIncreaseTime)
                {
                    gm.ExpandSpace();
                    updateTime = 0;
                }

                currentTime++;
                updateTime++;
                timeValue.text = (currentTime * Time.timeScale).ToString();
            }
            
            yield return new WaitForSecondsRealtime(1.0f);
        }

        if (currentTime >= maxTime)
        {
            Debug.Log("Time's up!");
            currentTime = maxTime;
            timeValue.text = (currentTime * Time.timeScale).ToString();
            gm.EndGame();
        }
    }
}
