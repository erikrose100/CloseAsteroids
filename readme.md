# CloseAsteroids C# Console App

Uses NASA's [SBDB Close-Approach Data API](https://ssd-api.jpl.nasa.gov/doc/cad.html) to return a list of asteroids and comets that ahve made NEO close-approaches within a given distance and time range:

```sh
dotnet run <start-date> <end-date> <distance(au)>
dotnet run 2099-01-01 2100-01-01 0.2
```

Example output:
```
dotnet run 2099-12-30 2100-01-01 0.2
Date: Wednesday, December 30, 2099
        Asteroid: 2019 XB
        Time: 23:02:00:000
        Distance: 0.084024385

Date: Thursday, December 31, 2099
        Asteroid: 2017 WS12
        Time: 05:03:00:000
        Distance: 0.14148647

Date: Thursday, December 31, 2099
        Asteroid: 2010 XB24
        Time: 21:00:00:000
        Distance: 0.1261884
```