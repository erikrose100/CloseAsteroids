namespace CloseAsteroids;

public static class BodyLookup
{
    public static readonly Dictionary<string, string> BodyDict = new()
        {
            { "Mercury", "Merc" },
            { "Venus", "Venus" },
            { "Earth", "Earth" },
            { "Mars", "Mars" },
            { "Jupiter", "Juptr" },
            { "Saturn", "Satrn" },
            { "Uranus", "Urnus" },
            { "Neptune", "Neptn" },
            { "Pluto", "Pluto" },
            { "Moon", "Moon" }
        };

    public static readonly Dictionary<string, double> PlanetsSemiMajorAxis = new()
        {
            { "Mercury", 0.39 },
            { "Venus", 0.72 },
            { "Earth", 1.00 },
            { "Mars", 1.52 },
            { "Jupiter", 5.20 },
            { "Saturn", 9.54 },
            { "Uranus", 19.19 },
            { "Neptune", 30.06 },
            { "Pluto", 39.5 },
            { "Moon", 0 }
        };
}