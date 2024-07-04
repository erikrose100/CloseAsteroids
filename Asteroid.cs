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
    public double? SemiMajorAxis { get; set; }
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

public class Element
{
    public string? sigma { get; set; }
    public string? name { get; set; }
    public string? units { get; set; }
    public string? value { get; set; }
    public string? label { get; set; }
    public string? title { get; set; }
}

public class OrbitObject
{
    public string? kind { get; set; }
    public bool? pha { get; set; }
    public bool? neo { get; set; }
    public string? spkid { get; set; }
    public OrbitClass? orbit_class { get; set; }
    public string? fullname { get; set; }
    public string? orbit_id { get; set; }
    public string? des { get; set; }
    public object? prefix { get; set; }
}

public class Orbit
{
    public string? soln_date { get; set; }
    public string? n_dop_obs_used { get; set; }
    public int n_obs_used { get; set; }
    public string? moid { get; set; }
    public string? comment { get; set; }
    public string? not_valid_before { get; set; }
    public string? t_jup { get; set; }
    public string? last_obs { get; set; }
    public string? data_arc { get; set; }
    public string? sb_used { get; set; }
    public string? producer { get; set; }
    public required List<Element> elements { get; set; }
    public string? orbit_id { get; set; }
    public string? condition_code { get; set; }
    public string? first_obs { get; set; }
    public string? cov_epoch { get; set; }
    public string? rms { get; set; }
    public string? not_valid_after { get; set; }
    public string? moid_jup { get; set; }
    public string? two_body { get; set; }
    public string? epoch { get; set; }
    public string? source { get; set; }
    public string? pe_used { get; set; }
    public List<OrbitObject>? model_pars { get; set; }
    public string? equinox { get; set; }
    public string? n_del_obs_used { get; set; }
}

public class OrbitClass
{
    public string? code { get; set; }
    public string? name { get; set; }
}

public class SmallBodyResponse
{
    public Signature? signature { get; set; }
    public OrbitObject? orbitObject { get; set; }
    public Orbit? orbit { get; set; }
}