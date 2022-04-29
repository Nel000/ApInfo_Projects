public struct Meal
{
    public int Score { get; }
    public int PrepTme { get; }

    public Meal(int score, int prepTime)
    {
        Score = score;
        PrepTme = prepTime;
    }
}
