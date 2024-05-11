using System.CommandLine;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloseAsteroids;

public static partial class Program
{
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("https://ssd-api.jpl.nasa.gov/cad.api"),
    };

    static void ConsoleOutput(List<Asteroid> asteroids, string body)
    {
        if (asteroids is null || !asteroids.Any())
        {
            if (string.Equals(body, "Earth"))
            {
                Console.WriteLine("No NEO close approaches detected in this time range.");
            }
            else
            {
                Console.WriteLine(string.Format("No asteroid close approaches for {0} detected in this time range.", body));
            }
            return;
        }

        var output = new StringBuilder();
        foreach (Asteroid asteroid in asteroids)
        {
            output.AppendFormat("Date: {0}\n\t", asteroid.CloseApproachTime?.ToString("D"));
            output.AppendFormat("Asteroid: {0}\n\t", asteroid.AsteroidDesignation);
            output.AppendFormat("Time: {0}\n\t", asteroid.CloseApproachTime?.ToString("HH:mm"));
            output.AppendFormat("Distance: {0}\n", asteroid.ApproachDistance);
            if (!string.Equals(body, "Earth")) { output.AppendFormat("\tBody: {0}\n", body); }
            output.Append('\n');
        }
        Console.WriteLine(output.ToString().TrimEnd());
    }

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(CloseApproachDataResponse), GenerationMode = JsonSourceGenerationMode.Metadata)]
    internal partial class SourceGenerationContext : JsonSerializerContext
    {
    }

    [JsonSourceGenerationOptions(WriteIndented = true)]
    [JsonSerializable(typeof(ReturnAsteroids), GenerationMode = JsonSourceGenerationMode.Metadata)]
    internal partial class SourceGenerationContextSerializer : JsonSerializerContext
    {
    }

    static async Task Main(string[] args)
    {
        var dateMin = new Option<DateTime>(
            name: "--date-min",
            description: "Minimum date to return close approaches for (start date).",
            getDefaultValue: () => DateTime.UtcNow)
        {
            Arity = ArgumentArity.ExactlyOne
        };

        var dateMax = new Option<DateTime>(
            name: "--date-max",
            description: "Maximum date to return close approaches for (end date).",
            getDefaultValue: () => DateTime.UtcNow.AddDays(30))
        {
            Arity = ArgumentArity.ExactlyOne
        };

        var distMax = new Option<string>(
            name: "--dist-max",
            description: "Maximum distance of return close approaches to return.",
            getDefaultValue: () => "0.2")
        {
            Arity = ArgumentArity.ExactlyOne
        };

        var body = new Option<string>(
            name: "--body",
            description: "Maximum distance of return close approaches to return.",
            getDefaultValue: () => "Earth")
        {
            Arity = ArgumentArity.ExactlyOne
        };

        body.AddAlias("-b");

        var output = new Option<string>(
            name: "--output",
            description: "Sets serialization type for stdout.")
        {
            Arity = ArgumentArity.ExactlyOne
        };

        output.AddAlias("-o");

        var delimiter = new Option<string>(
            name: "--delimiter",
            description: "What delimiter string to use for table output.",
            getDefaultValue: () => ",")
        {
            Arity = ArgumentArity.ExactlyOne
        };

        var rootCommand = new RootCommand("CLI app that returns close approaches of asteroids for a given date range.")
        {
            output,
            body,
            dateMin,
            dateMax,
            distMax,
            delimiter
        };

        rootCommand.SetHandler(async (output, body, dateMin, dateMax, distMax, delimiter) =>
        {
            var dateMinString = dateMin.ToString("yyyy-MM-dd");
            var dateMaxString = dateMax.ToString("yyyy-MM-dd");
            var bodyString = BodyLookup.BodyDict[body];
            var response = await sharedClient.GetAsync($"?date-min={dateMinString}&date-max={dateMaxString}&dist-max={distMax}&body={bodyString}");
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var json = JsonSerializer.Deserialize<CloseApproachDataResponse>(jsonResponse, SourceGenerationContext.Default.CloseApproachDataResponse);
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

            switch (output)
            {
                case "json":
                    var asteroidOutput = new ReturnAsteroids
                    {
                        Timestamp = DateTime.Now,
                        Asteroids = asteroids
                    };
                    var jsonString = JsonSerializer.Serialize(asteroidOutput!, SourceGenerationContextSerializer.Default.ReturnAsteroids);
                    Console.WriteLine(jsonString);
                    break;
                
                case "table":
                    var tableOutput = new StringBuilder();
                    tableOutput.AppendFormat("Date{0}Asteroid{0}Time{0}Distance{0}Body\n", delimiter);
                    foreach (Asteroid asteroid in asteroids!) 
                    {
                        tableOutput.AppendFormat("{1}{0}{2}{0}{3}{0}{4}{0}{5}\n", delimiter, asteroid.CloseApproachTime?.ToString("s"), asteroid.AsteroidDesignation, asteroid.CloseApproachTime?.ToString("HH:mm"),  asteroid.ApproachDistance, body.Trim());   
                    }
                    Console.WriteLine(tableOutput.ToString().TrimEnd());
                    break;

                default:
                    ConsoleOutput(asteroids!, body);
                    break;
            }
        },
        output, body, dateMin, dateMax, distMax, delimiter);

        await rootCommand.InvokeAsync(args);
    }
}
