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

    [SerializeField] private bool isOnBalcony;

    [SerializeField] private List<Meal> preparedMeals = new List<Meal>();

    private Meal hamburger = new Meal(20, 30);
    private Meal pasta = new Meal(20, 30);
    private Meal pizza = new Meal(20, 30);
    private Meal salad = new Meal(20, 30);
    private Meal spaghettiAndMeatballs = new Meal(20, 30);

    public Meal[] meals = new Meal[5];

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        waiter = FindObjectOfType<Waiter>();

        meals[0] = hamburger;
        meals[1] = pasta;
        meals[2] = pizza;
        meals[3] = salad;
        meals[4] = spaghettiAndMeatballs;
    }

    private void OnMouseDown()
    {
        // Move to balcony
        Debug.Log("Clicked balcony");

        if (!waiter.GetComponent<Waiter>().IsMoving && !isOnBalcony)
            StartCoroutine(waiter.GetComponent<Waiter>().Move(
                waitPosition.transform.position));
        else
        {
            gm.IsLocked = true;
            menu.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("On balcony");
            isOnBalcony = true;

            menu.SetActive(true);
            gm.IsLocked = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            isOnBalcony = false;
        }
    }

    public void SelectMeal(int index)
    {
        StartCoroutine(PrepareMeal(meals[index]));
    }

    private IEnumerator PrepareMeal(Meal preparedMeal)
    {
        yield return null;
        preparedMeals.Add(preparedMeal);
        PositionMeal(0);
    }

    private void PositionMeal(int index)
    {
        if (positions[index].IsOccupied && index <= positions.Length)
        {
            PositionMeal(index + 1);
        }
        else if (index > positions.Length)
        {
            // Wait until there is a free position
        }
        else
        {
            positions[index].IsOccupied = true;
            Instantiate(gm.Meals[index], positions[index].transform);
        }
    }
}
