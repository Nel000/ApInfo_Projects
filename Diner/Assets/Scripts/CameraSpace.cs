using UnityEngine;

public class CameraSpace : MonoBehaviour
{
    private const float areaIncrease = 160;

    [SerializeField] private float scaleUpdateX, scaleUpdateY;
    [SerializeField] private float widthDiff, heightDiff;

    private void Start()
    {
        float area = transform.localScale.x * transform.localScale.y;
        float increasedArea = area + areaIncrease;
        scaleUpdateX = 
            transform.localScale.x * Mathf.Sqrt(increasedArea / area);
        scaleUpdateY = 
            transform.localScale.y * Mathf.Sqrt(increasedArea / area);
        widthDiff = scaleUpdateX - transform.localScale.x;
        heightDiff = scaleUpdateY - transform.localScale.y;
    }

    public void ExpandSpace()
    {
        float newWidth = transform.localScale.x + widthDiff;
        float newHeight = transform.localScale.y + heightDiff;

        transform.position = new Vector2(
            transform.position.x - (widthDiff/ 2f),
            transform.position.y - (heightDiff / 2f));
        transform.localScale = new Vector2(newWidth, newHeight);
    }
}
