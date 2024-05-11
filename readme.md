# CloseAsteroids C# Console App
AOT compiled C# Console app. Chiseled Ubuntu and Alpine Dockerfiles with minimal dependencies provided for optional containerization.

Uses NASA's [SBDB Close-Approach Data API](https://ssd-api.jpl.nasa.gov/doc/cad.html) to return a list of asteroids and comets that have made NEO close-approaches within a given distance and time range.

Example usage (ran from the root dir of this repo):

```sh
dotnet run --date-min <start-date> --date-max <end-date> --dist-max <distance(au)>
dotnet run --date-min 2099-01-01 --date-max 2100-01-01 --dist-max 0.2
```

You can also call the binary directly:
```sh
./CloseAsteroids --date-min 2063-05-12 --date-max 2065-02-03 --dist-max 0.3
# after adding to your PATH
CloseAsteroids --date-min 2063-05-12 --date-max 2065-02-03 --dist-max 0.3
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
## Output Options
### json
You can use the `--output=json` option to output json to stdout:
```sh
dotnet run --output=json
CloseAsteroids -o=json
```
Example output:
```json
{
  "Timestamp": "2024-05-09T03:13:18.0804695+00:00",
  "Asteroids": [
    {
      "AsteroidDesignation": "410777",
      "OrbitID": "107",
      "JDTime": 2488063.083053552,
      "CloseApproachTime": "2099-12-25T14:00:00",
      "ApproachDistance": 0.1498545,
      "DistMin": 0.1495639,
      "DistMax": 0.15014513,
      "VRel": 21.763435,
      "VInf": 21.76277,
      "TSigUncertainty": "00:21",
      "AbsoluteMagnitude": 22.18
    },
    {
      "AsteroidDesignation": "620089",
      "OrbitID": "21",
      "JDTime": 2488064.309669096,
      "CloseApproachTime": "2099-12-26T19:26:00",
      "ApproachDistance": 0.07969794,
      "DistMin": 0.07968595,
      "DistMax": 0.079709955,
      "VRel": 34.890934,
      "VInf": 34.890156,
      "TSigUncertainty": "00:02",
      "AbsoluteMagnitude": 19.43
    }, [...]
```

### table
You can use the `--output=table` option to output json to stdout:
```sh
dotnet run --output=table
CloseAsteroids -o table
```
Example output:
```
Date,Asteroid,Time,Distance,Body
2099-12-25T14:00:00,410777,14:00,0.1498545,Venus
2099-12-26T19:26:00,620089,19:26,0.07969794,Venus
2099-12-27T11:17:00,2023 BM4,11:17,0.06353373,Venus
2099-12-27T13:49:00,154658,13:49,0.12515673,Venus
2099-12-29T04:08:00,2015 NU13,04:08,0.06993372,Venus
2099-12-29T04:10:00,2016 EE157,04:10,0.110200495,Venus
2099-12-29T17:09:00,468910,17:09,0.076191306,Venus
2099-12-29T20:32:00,2017 EN,20:32,0.08239899,Venus
2099-12-31T05:30:00,2016 BC14,05:30,0.1080818,Venus
```
You can change the delimiter using the `--delimiter` option:
```sh
CloseAsteroids --date-min=2099-12-25 --date-max=2100-01-01 --dist-max=0.2 --body=Mars --output=table --delimiter="/"
```
```
Date/Asteroid/Time/Distance/Body
2099-12-25T09:08:00/612267/09:08/0.023517134/Mars
2099-12-29T09:28:00/2006 WE129/09:28/0.088202335/Mars
```

## Running in Docker
> [!NOTE]  
`dotnet publish` is setup in the Dockerfile to AOT compile the app for linux_x64 so your host will need to be compatible with this. If you're on ARM you either need to [setup your environment for cross-compilation](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/cross-compile) or change `dotnet publish -r linux-x64` to `dotnet publish -r linux-arm64`.

You can build and run the image defined in this repo's Dockerfile by running the following:

```sh
docker build -t close-asteroids -f Dockerfile .
docker run close-asteroids
```

You can pass arguments using the ASTEROIDARGS env variable like so:
```sh
docker run -e ASTEROIDARGS="--date-min=2099-12-25 --date-max=2100-01-01 --dist-max=0.2 --body=Venus" close-asteroids
```

If you'd like to use the Alpine-based container, build and run the image like so:

```sh
docker build -t close-asteroids-alpine -f Dockerfile.Alpine .
docker run close-asteroids-alpine
```
