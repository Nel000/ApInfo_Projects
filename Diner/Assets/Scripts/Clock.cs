using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour, IUIElement
{
    private const int diffIncreaseTime = 10, updateValue = 1;

    private GameManager gm;
    private Rating rating;

    [SerializeField] private Image fillImg;
    public Image FillImg => fillImg;

    [SerializeField] private Text timeValue;

    [SerializeField] private float currentValue;
    public float CurrentValue => currentValue;

    public float TotalWeight { get; }

    [SerializeField] private int currentTime, maxTime;
    public int CurrentTime => currentTime;

    public int Index { get; }

    public bool Filled { get; }

    public bool Completed { get; }
    
    public bool Depleted { get; }

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        rating = FindObjectOfType<Rating>();

        currentTime = -1;

        StartCoroutine(RaiseTime(diffIncreaseTime));
        StartCoroutine(Fill(diffIncreaseTime));
    }

    public IEnumerator Fill(int value, bool complete = false)
    {
        float valueModifier = 0;

        while (currentTime < value)
        {
            if (!rating.InEmergency)
            {
                currentValue = Mathf.Clamp(
                    currentValue + updateValue * Time.deltaTime, 
                    0, diffIncreaseTime);

                valueModifier = currentValue / diffIncreaseTime;
                fillImg.fillAmount = valueModifier;

                yield return null;
            }
        }

        currentValue = 0;

        yield return new WaitForEndOfFrame();
        StartCoroutine(Fill(diffIncreaseTime));
    }

    private IEnumerator RaiseTime(int value)
    {
        int updateTime = 0;

        while (currentTime < value)
        {
            if (!rating.InEmergency)
            {
                if (updateTime >= value)
                {
                    gm.ExpandSpace();
                    updateTime = 0;
                }

                currentTime++;
                updateTime++;
                timeValue.text = (currentTime * Time.timeScale).ToString();

                yield return new WaitForSeconds(1.0f);
            }
        }

        currentTime = -1;

        /*if (currentTime >= maxTime)
        {
            Debug.Log("Time's up!");
            currentTime = maxTime;
            timeValue.text = (currentTime * Time.timeScale).ToString();
            gm.EndGame();
        }*/

        StartCoroutine(RaiseTime(diffIncreaseTime));
    }
}
