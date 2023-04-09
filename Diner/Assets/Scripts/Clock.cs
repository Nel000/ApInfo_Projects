using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour, IUIElement
{
    private const int diffIncreaseTime = 60, updateValue = 1;
    private const float clockSizeMultiplier = 1.25f;

    private GameManager gm;
    private Rating rating;

    [SerializeField] private Image fillImg;
    public Image FillImg => fillImg;

    [SerializeField] private Text timeValue;

    [SerializeField] private float currentValue;
    public float CurrentValue => currentValue;

    [SerializeField] private float nextValue;

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
                    0, value);

                valueModifier = currentValue / value;
                fillImg.fillAmount = valueModifier;

                yield return null;
            }
        }

        currentValue = 0;

        yield return new WaitForEndOfFrame();
        StartCoroutine(Fill((int)nextValue));
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
        nextValue = value * clockSizeMultiplier;

        StartCoroutine(RaiseTime((int)nextValue));
    }
}
