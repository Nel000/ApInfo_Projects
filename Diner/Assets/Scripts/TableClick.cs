using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableClick : MonoBehaviour
{
    [SerializeField] private Table table;

    // Start is called before the first frame update
    void Start()
    {
        table = GetComponentInParent<Table>();
    }

    private void OnMouseDown()
    {
        table.Clicked = true;
    }
}
