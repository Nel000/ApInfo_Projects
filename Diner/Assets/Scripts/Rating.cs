using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rating : MonoBehaviour
{
    private const int starAmount = 3;

    private IUIElement[] stars = new IUIElement[starAmount];
    
    [SerializeField] private int currentStar = 1, previousStar = 1;

    [SerializeField] private bool emergencyMode;

    private void Start()
    {
        for (int i = 0; i < starAmount; i++)
        {
            stars[i] = GameObject.Find($"Star {i + 1}").GetComponent<Star>();
        }
    }

    public void UpdateRating(int value, bool critic)
    {
        if (currentStar > 0) previousStar = currentStar;

        currentStar = DetermineStar(value, critic);

        if (currentStar > 0)
        {
            if (critic)
            {
                if (value > 0)
                    StartCoroutine(stars[currentStar - 1].Fill(
                        (int)stars[currentStar - 1].TotalWeight - 
                        (int)stars[currentStar - 1].CurrentValue, true));
                else
                    StartCoroutine(stars[currentStar - 1].Fill(
                        -(int)stars[currentStar - 1].CurrentValue));
            }
            else StartCoroutine(stars[currentStar - 1].Fill(value));
        }
    }

    private int DetermineStar(int value, bool critic)
    {
        if (value > 0 && stars[previousStar - 1].Filled 
            && !stars[previousStar - 1].Completed)
        {
            if (critic) return previousStar;
            return 0;
        }

        for (int i = 0; i < starAmount; i++)
        {
            if (!stars[i].Completed)
            {
                if (value > 0)
                {
                    if (stars[i].Filled && critic || !stars[i].Filled && !critic)
                        return stars[i].Index;
                }
                else
                {
                    if (stars[i].Depleted)
                    {
                        if (stars[i].Index == 1)
                        {
                            emergencyMode = true;
                        }
                        else return stars[i - 1].Index;
                    }
                    else
                    {
                        return stars[i].Index;
                    }
                }
            }
            else if (stars[i].Completed)
            {
                if (stars[i].Index == starAmount && value < 0)
                {
                    return stars[i].Index;
                }
            }
        }

        return 0;
    }
}
