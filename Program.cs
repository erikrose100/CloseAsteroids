using System.Net.Http.Json;
using System.Text.Json;
using System.Xml;

namespace CloseAsteroids;

class Program
{
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://ssd-api.jpl.nasa.gov/cad.api"),
    };

    static async Task Main(string[] args)
    {
        //1900-01-01
        var dateMin = DateTime.Parse(args[0]).ToString("yyyy-MM-dd");
        //2100-01-01
        var dateMax = DateTime.Parse(args[1]).ToString("yyyy-MM-dd");
        //0.2
        var distMax = args[2];
        var response = await sharedClient.GetAsync($"?date-min={dateMin}&date-max={dateMax}&dist-max={distMax}");
        var jsonResponse = await response.Content.ReadAsStringAsync();

        var json = JsonSerializer.Deserialize<CloseApproachDataResponse>(jsonResponse);
        var asteroids = json?.data?
            .ConvertAll(x => new Asteroid
            {
                AsteroidDesignation = x[0],
                OrbitID = x[1],
                JDTime = x[2] != null ? Double.Parse(x[2]) : null,
                CloseApproachTime = x[3] != null ? DateTime.Parse(x[3]) : null,
                ApproachDistance = x[4] != null ? Single.Parse(x[4]) : null,
                DistMin = x[5] != null ? Single.Parse(x[5]) : null,
                DistMax = x[6] != null ? Single.Parse(x[6]) : null,
                VRel = x[7] != null ? Single.Parse(x[7]) : null,
                VInf = x[8] != null ? Single.Parse(x[8]) : null,
                TSigUncertainty = x[9],
                AbsoluteMagnitude = x[10] != null ? Single.Parse(x[10]) : null
            });

        if (asteroids is not null)
        {
            foreach (Asteroid asteroid in asteroids)
            {
                var output = string.Format("Date: {0}\n\tAsteroid: {1}\n\tTime: {2}\n\tDistance: {3}\n\t", asteroid.CloseApproachTime?.ToString("D"), asteroid.AsteroidDesignation, asteroid.CloseApproachTime?.ToString("HH:mm"), asteroid.ApproachDistance);
                Console.WriteLine(output);
            }
        }
        else
        {
            Console.WriteLine("No NEO close approaches detected in this time range.");
        }

    }
}
