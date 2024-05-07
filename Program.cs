using System.CommandLine;
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
        var dateMin = new Option<DateTime>(
            name: "--date-min",
            description: "Minimum date to return close approaches for (start date).",
            getDefaultValue: () => DateTime.UtcNow);

        var dateMax = new Option<DateTime>(
            name: "--date-max",
            description: "Maximum date to return close approaches for (end date).",
            getDefaultValue: () => DateTime.UtcNow.AddDays(30));

        var distMax = new Option<string>(
            name: "--dist-max",
            description: "Maximum distance of return close approaches to return.",
            getDefaultValue: () => "0.2");

        var body = new Option<string>(
            name: "--body",
            description: "Maximum distance of return close approaches to return.",
            getDefaultValue: () => "Earth");

        var rootCommand = new RootCommand("CLI app that returns close approaches of asteroids for a given date range.")
        {
            body,
            dateMin,
            dateMax,
            distMax
        };

        rootCommand.SetHandler(async (body, dateMin, dateMax, distMax) =>
        {
            var dateMinString = dateMin.ToString("yyyy-MM-dd");
            var dateMaxString = dateMax.ToString("yyyy-MM-dd");
            var bodyString = BodyLookup.BodyDict[body];
            var response = await sharedClient.GetAsync($"?date-min={dateMinString}&date-max={dateMaxString}&dist-max={distMax}&body={bodyString}");
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
                    var bodyOutput = body != "Earth" ? output + string.Format("Body: {0}\n\t", body) : output;
                    Console.WriteLine(bodyOutput);
                }
            }
            else
            {
                if (body == "Earth")
                {
                    Console.WriteLine("No NEO close approaches detected in this time range.");
                }
                else
                {
                    Console.WriteLine(string.Format("No asteroid close approaches for {0} detected in this time range.", body));
                }
            }
        },
        body, dateMin, dateMax, distMax);

        await rootCommand.InvokeAsync(args);
    }
}
