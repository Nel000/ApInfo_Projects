using System.Linq;
using UnityEngine;

public class LineSpot : MonoBehaviour
{
    private GameManager gm;

    [SerializeField] private Vector2 location;
    public Vector2 Location => location;

    [SerializeField] private int position;
    public int Position { get => position; set => position = value; }

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        location = new Vector2(transform.position.x, transform.position.y);

        DefinePosition();
    }

    public void DefinePosition()
    {
        position = gm.LineSpotList.Count() + 1;

        name = $"Line Spot {position}";
    } 
}
