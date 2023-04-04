using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rating : MonoBehaviour
{
    private const int starAmount = 3;

    private IUIElement[] stars = new IUIElement[starAmount];
    
    [SerializeField] private int currentStar;

    private void Start()
    {
        for (int i = 0; i < starAmount; i++)
        {
            stars[i] = GameObject.Find($"Star {i + 1}").GetComponent<Star>();
        }
    }

    public void UpdateRating(int value)
    {
        currentStar = DetermineStar();

        StartCoroutine(stars[currentStar - 1].Fill(value));
    }

    private int DetermineStar()
    {
        for (int i = 0; i < starAmount; i++)
        {
            if (!stars[i].Filled)
            {
                return stars[i].Index;
            }
        }

        return 0;
    }
}
