using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour, IUIElement
{
    private const float updateTime = 0.5f;

    [SerializeField] private Image fillImg;
    public Image FillImg => fillImg;

    [SerializeField] private float currentValue;
    public float CurrentValue => currentValue;

    [SerializeField] private float totalWeight;
    public float TotalWeight => totalWeight;

    [SerializeField] private int index;
    public int Index => index;

    [SerializeField] private bool filled;
    public bool Filled => filled;

    [SerializeField] private bool working;

    public IEnumerator Fill(int value)
    {
        float startValue = Mathf.Round(currentValue);
        float valueModifier = 0;

        float changePerSecond = totalWeight / (updateTime * 10);

        if (working)
        {
            while (working)
                yield return new WaitForEndOfFrame();
        }

        working = true;
        
        while (value > 0 && currentValue < startValue + value
            || value < 0 && currentValue > startValue + value)
        {
            Debug.Log(fillImg.fillAmount);

            if (value < 0)
                currentValue = Mathf.Clamp(
                    currentValue - changePerSecond * Time.deltaTime, 
                    0, totalWeight);
            else
                currentValue = Mathf.Clamp(
                    currentValue + changePerSecond * Time.deltaTime, 
                    0, totalWeight);

            valueModifier = currentValue / totalWeight;
            fillImg.fillAmount = valueModifier;

            yield return null;
        }

        if (currentValue >= totalWeight)
        {
            currentValue = totalWeight;
            filled = true;
        }
        else if (currentValue < totalWeight && filled)
        {
            filled = false;
        }

        if (currentValue < 0)
        {
            if (index == 1)
            {

            }
            else
            {

            }
        }

        working = false;
    }
}
