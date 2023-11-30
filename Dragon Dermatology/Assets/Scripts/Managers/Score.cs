public struct Score {
    public int CoinsCollected;
    public int RegularScalesCollected;
    public int FireScalesCollected;
    public int WaterScalesCollected;
    public int SeenProudDragons;
    public int SeenCleanDragons;
    public int SeenUnhappyDragons;
    public int UnseenDragons;

    public Score(
        int coinsCollected,
        int regularScalesCollected,
        int fireScalesCollected,
        int waterScalesCollected,
        int seenProudDragons,
        int seenCleanDragons,
        int seenUnhappyDragons,
        int unseenDragons
    ) {
        CoinsCollected = coinsCollected;
        RegularScalesCollected = regularScalesCollected;
        FireScalesCollected = fireScalesCollected;
        WaterScalesCollected = waterScalesCollected;
        SeenProudDragons = seenProudDragons;
        SeenCleanDragons = seenCleanDragons;
        SeenUnhappyDragons = seenUnhappyDragons;
        UnseenDragons = unseenDragons;
    }

    public override string ToString()
    {
        return $"{CoinsCollected} coins. " +
            $"{RegularScalesCollected} regular. " +
            $"{FireScalesCollected} fire. " +
            $"{WaterScalesCollected} water. " +
            $"{SeenProudDragons} proud. " +
            $"{SeenCleanDragons} clean. " +
            $"{SeenUnhappyDragons} unhappy. " +
            $"{UnseenDragons} unseen."
        ;
    }

    public Score Add(Score other)
    {
        return new Score(
            CoinsCollected + other.CoinsCollected,
            RegularScalesCollected + other.RegularScalesCollected,
            FireScalesCollected + other.FireScalesCollected,
            WaterScalesCollected + other.WaterScalesCollected,
            SeenProudDragons + other.SeenProudDragons,
            SeenCleanDragons + other.SeenCleanDragons,
            SeenUnhappyDragons + other.SeenUnhappyDragons,
            UnseenDragons + other.UnseenDragons
        );
    }
}