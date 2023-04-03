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
        StartCoroutine(stars[0].Fill(value));
    }
}
