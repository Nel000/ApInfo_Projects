using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Balcony : MonoBehaviour
{
    private GameManager gm;
    private Waiter waiter;

    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject[] menuMeals;

    [SerializeField] private PlacementPoint[] positions;
    [SerializeField] private Transform waitPosition;

    public bool IsOnBalcony { get; private set; }

    [SerializeField] private List<Meal> mealsToPrepare = new List<Meal>();
    [SerializeField] private List<Meal> preparedMeals = new List<Meal>();

    private Meal hamburger = new Meal(30, 30);
    private Meal pasta = new Meal(30, 40);
    private Meal pizza = new Meal(30, 70);
    private Meal salad = new Meal(30, 20);
    private Meal spaghettiAndMeatballs = new Meal(30, 50);

    public Meal[] meals = new Meal[5];

    [SerializeField] private Text[] time = new Text[5];

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        waiter = FindObjectOfType<Waiter>();

        meals[0] = hamburger;
        meals[1] = pasta;
        meals[2] = pizza;
        meals[3] = salad;
        meals[4] = spaghettiAndMeatballs;

        for (int i = 0; i < meals.Length; i++)
        {
            time[i].text = ($"Time: {meals[i].PrepTme.ToString()}s");
        }
    }

    private void OnMouseDown()
    {
        // Move to balcony
        Debug.Log("Clicked balcony");

        if (!waiter.GetComponent<Waiter>().IsMoving && !IsOnBalcony)
            StartCoroutine(waiter.GetComponent<Waiter>().Move(
                waitPosition.transform.position));
        else
        {
            if (!gm.IsLocked)
            {
                gm.IsLocked = true;
                menu.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("On balcony");
            IsOnBalcony = true;

            menu.SetActive(true);
            gm.IsLocked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            IsOnBalcony = false;
        }
    }

    public void SelectMeal(int index)
    {
        Debug.Log($"Preparing meal #{index + 1}.");
        StartCoroutine(PrepareMeal(meals[index], index));
    }

    private IEnumerator PrepareMeal(Meal mealToPrepare, int index)
    {
        if(mealsToPrepare.Count < positions.Length + 2)
        {
            mealsToPrepare.Add(mealToPrepare);
            yield return new WaitForSeconds(mealToPrepare.PrepTme);
            StartCoroutine(PositionMeal(mealToPrepare, index, 0));
        }
        else
        {
            Debug.Log($"Can't prepare meal #{index + 1}.");
        }
    }

    private IEnumerator PositionMeal(
        Meal preparedMeal, int mealIndex, int balconyIndex)
    {
        if (balconyIndex < positions.Length 
            && positions[balconyIndex].IsOccupied)
        {
            StartCoroutine(
                PositionMeal(preparedMeal, mealIndex, balconyIndex + 1));
        }
        else if (balconyIndex >= positions.Length)
        {
            // Wait until there is a free position
            yield return new WaitForSeconds(1.0f);
            Debug.Log($"Waiting to position meal #{mealIndex + 1}...");
            StartCoroutine(
                PositionMeal(preparedMeal, mealIndex, 0));
        }
        else
        {
            Debug.Log($"Meal positioned at position #{balconyIndex + 1}.");
            mealsToPrepare.Remove(preparedMeal);
            preparedMeals.Add(preparedMeal);
            positions[balconyIndex].IsOccupied = true;
            
            positions[balconyIndex].PlacedMeal = Instantiate(
                gm.Meals[mealIndex], positions[balconyIndex].transform);
        }
    }
}
