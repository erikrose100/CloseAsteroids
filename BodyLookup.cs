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
}