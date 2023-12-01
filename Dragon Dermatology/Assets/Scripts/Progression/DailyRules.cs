using System.Collections.Generic;

public static class DailyRules {
    // The likelihood of each kind of dragon spawning on a day. If a day isn't listed, dragons have equal weight.
    // Regular, Water, Fire
    private static readonly Dictionary<int, int[]> SpawnWeightsByDay = new Dictionary<int, int[]> {
        [1] = new int[] { 1, 0, 0 },
        [2] = new int[] { 2, 1, 0 },
        [3] = new int[] { 2, 2, 1 }
    };

    // Regular, Water, Fire
    public static int[] SpawnWeights(int day)
    {
        int[] weights;
        if (!SpawnWeightsByDay.TryGetValue(day, out weights)) {
            weights = new int[] { 1, 1, 1 };
        }

        return weights;
    }
}