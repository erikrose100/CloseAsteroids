namespace CloseAsteroids;

public class Asteroid
{
    public string? AsteroidDesignation { get; set; }
    public string? OrbitID { get; set; }
    public double? JDTime { get; set; }
    public DateTime? CloseApproachTime { get; set; }
    public float? ApproachDistance { get; set; }
    public float? DistMin { get; set; }
    public float? DistMax { get; set; }
    public float? VRel { get; set; }
    public float? VInf { get; set; }
    public string? TSigUncertainty { get; set; }
    public float? AbsoluteMagnitude { get; set; }
}

public class ReturnAsteroids
{
    public DateTime Timestamp { get; set; }
    public List<Asteroid>? Asteroids { get; set; }
}

public class Signature
{
    public string? source { get; set; }
    public string? version { get; set; }
}

public class CloseApproachDataResponse
{
    public Signature? Signature { get; set; }
    public int count { get; set; }
    public string[]? fields { get; set; }
    public List<List<string>>? data { get; set; }
}