using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Rating rating;

    [SerializeField] private Text scoreValue, endScoreValue;

    [SerializeField] private GameObject mainCanvas, endCanvas;

    private int currentScore;

    private void Start()
    {
        currentScore = 0;
    }

    public void ProcessScore(int value)
    {
        currentScore += value;
        scoreValue.text = currentScore.ToString();
        rating.UpdateRating(value);
    }

    public void EndScore()
    {
        mainCanvas.SetActive(false);
        endCanvas.SetActive(true);
        endScoreValue.text = $"Score: {currentScore}";
    }
}
