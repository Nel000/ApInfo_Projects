using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WaitingSlot : MonoBehaviour
{
    private MealPreparation mealPrep;

    [SerializeField] private Image mealImage;

    [SerializeField] private Text prepText;

    private int prepTime;

    public WaitingSlot(Image mealImage, int prepTime)
    {
        this.mealImage = mealImage;
        this.prepTime = prepTime;
    }

    public void DefineMeal(Image mealImage, int prepTime)
    {
        this.mealImage.sprite = mealImage.sprite;
        this.mealImage.color = mealImage.color;
        this.prepTime = prepTime;
    }

    private void Start()
    {
        mealPrep = FindObjectOfType<MealPreparation>();

        StartCoroutine(WaitTime());
    }

    private IEnumerator WaitTime()
    {
        do
        {
            prepTime--;
            prepText.text = (prepTime * Time.timeScale).ToString() + "s";
            yield return new WaitForSecondsRealtime(1.0f);
        }
        while (prepTime > 0);

        if (prepTime <= 0)
        {
            mealPrep.RemoveMeal(this.gameObject);
            Destroy(this.gameObject);
        }
    }
}
