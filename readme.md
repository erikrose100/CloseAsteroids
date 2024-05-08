# CloseAsteroids C# Console App
AOT compiled C# Console app. Chiseled ubuntu Dockerfile with minimal dependencies provided for optional containerization.

Uses NASA's [SBDB Close-Approach Data API](https://ssd-api.jpl.nasa.gov/doc/cad.html) to return a list of asteroids and comets that have made NEO close-approaches within a given distance and time range:

```sh
dotnet run --date-min <start-date> --date-max <end-date> --dist-max <distance(au)>
dotnet run --date-min 2099-01-01 --date-max 2100-01-01 --dist-max 0.2
```

By default returns asteroid / comet close approaches for Earth but can specify other bodies using the `body` option:

```sh
dotnet run --date-min 2099-01-01 --date-max 2100-01-01 --dist-max 0.2 --body "Mercury"
```

You can see a list of available bodies in [BodyLookup.cs](BodyLookup.cs) or [in the "Body" column here](https://ssd-api.jpl.nasa.gov/doc/cad.html#cad_body_table).

Can be run without parameters and will return NEO close approaches for Earth in the next 30 days with a max distance of 0.2 au.

Example output:
```
dotnet run --date-min 2099-12-30 --date-max 2100-01-01 --dist-max 0.2
Date: Wednesday, December 30, 2099
        Asteroid: 2019 XB
        Time: 23:02
        Distance: 0.084024385

Date: Thursday, December 31, 2099
        Asteroid: 2017 WS12
        Time: 05:03
        Distance: 0.14148647

Date: Thursday, December 31, 2099
        Asteroid: 2010 XB24
        Time: 21:00
        Distance: 0.1261884
```

Example output when defining a body other than Earth:
```
dotnet run --date-min 2099-12-25 --date-max 2100-01-01 --dist-max 0.2 --body Mercury
Date: Friday, December 25, 2099
        Asteroid: 2023 BY
        Time: 13:00
        Distance: 0.067190215
        Body: Mercury

Date: Sunday, December 27, 2099
        Asteroid: 2013 UG1
        Time: 11:48
        Distance: 0.04223703
        Body: Mercury

Date: Thursday, December 31, 2099
        Asteroid: 518847
        Time: 15:13
        Distance: 0.06283979
        Body: Mercury
```
## Running in Docker
> [!NOTE]  
`dotnet publish` is setup in the Dockerfile to AOT compile the app for linux_x64 so your host will need to be compatible with this. If you're on ARM you either need to [setup your environment for cross-compilation](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/cross-compile) or change `dotnet publish -r linux-x64` to `dotnet publish -r linux-arm64`.

You can build and run the image defined in this repo's Dockerfile by running the following:

```sh
docker build -t close-asteroids -f Dockerfile .
docker run -it close-asteroids
```

You can pass arguments using the ASTEROIDARGS env variable like so:
```sh
docker run -e ASTEROIDARGS="--date-min=2099-12-25 --date-max=2100-01-01 --dist-max=0.2 --body=Venus" -it close-asteroids
```