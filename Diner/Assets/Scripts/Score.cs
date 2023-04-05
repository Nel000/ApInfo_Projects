using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Rating rating;

    [SerializeField] private Text scoreValue, endScoreValue;

    [SerializeField] private GameObject mainCanvas, endCanvas;

    private int currentScore;

    private void Start()
    {
        rating = FindObjectOfType<Rating>();

        currentScore = 0;
    }

    public void ProcessScore(int value, bool critic)
    {
        currentScore += value;
        scoreValue.text = currentScore.ToString();
        rating.UpdateRating(value, critic);
    }

    public void EndScore()
    {
        mainCanvas.SetActive(false);
        endCanvas.SetActive(true);
        endScoreValue.text = $"Score: {currentScore}";
    }
}
