using UnityEngine;

public class TableClick : MonoBehaviour
{
    [SerializeField] private Table table;

    private void Start()
    {
        table = GetComponentInParent<Table>();
    }

    private void OnMouseUpAsButton()
    {
        table.Clicked = true;
    }
}
