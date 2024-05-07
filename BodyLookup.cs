namespace CloseAsteroids;

public static class BodyLookup
{
    public static readonly Dictionary<string, string> BodyDict = new()
        {
            { "Merc", "Mercury" },
            { "Venus", "Venus" },
            { "Earth", "Earth" },
            { "Mars", "Mars" },
            { "Juptr", "Jupiter" },
            { "Satrn", "Saturn" },
            { "Urnus", "Uranus" },
            { "Neptn", "Neptune" },
            { "Pluto", "Pluto" },
            { "Moon", "Moon" }
        };
}