using System;
using UnityEngine;

public static class WeightedRandom {

    /// <summary>
    /// Randomly return an index from this array. Indices are weighted by their array value, making them more likely to
    /// be selected.
    /// </summary>
    public static int Index(int[] weights)
    {
        int sum = 0;
        foreach (int w in weights) {
            sum += w;
        }

        float r = UnityEngine.Random.value * sum;

        float v = 0;
        for (int i = 0; i < weights.Length; i++) {
            v += weights[i];
            if (v >= r) {
                return i;
            }
        }

        // We should never get here
        throw new Exception("WeightedSelection somehow did not find a value.");
    }
}