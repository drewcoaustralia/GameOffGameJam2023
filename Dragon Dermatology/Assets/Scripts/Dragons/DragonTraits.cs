using System.Collections.Generic;

public static class DragonTraits {
    // Cleanliness preferences
    public static readonly Dictionary<Species, float> CleanPreference = new Dictionary<Species, float> {
        [Species.Regular] = 0.5f,
        [Species.Water]   = 0.65f,
        [Species.Fire]    = 0.8f
    };

    // Polish preferences
    public static readonly Dictionary<Species, float> PolishPreference = new Dictionary<Species, float> {
        [Species.Regular] = 0.25f,
        [Species.Water]   = 0.5f,
        [Species.Fire]    = 0.8f
    };

    // The coins to pay if clean
    public static readonly Dictionary<Species, int> CoinsIfClean = new Dictionary<Species, int> {
        [Species.Regular] = 2,
        [Species.Water]   = 3,
        [Species.Fire]    = 4
    };

    // The coins to pay if polished
    public static readonly Dictionary<Species, int> CoinsIfPolished = new Dictionary<Species, int> {
        [Species.Regular] = 3,
        [Species.Water]   = 4,
        [Species.Fire]    = 5
    };
}