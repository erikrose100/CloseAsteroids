FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG args="--date-min 2099-12-25 --date-max 2100-01-01 --dist-max 0.2 --body Mercury"
ENV ENVARGS=$args
WORKDIR /source

COPY . .
RUN dotnet publish -c Release -o /app CloseAsteroids.csproj

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT /app/CloseAsteroids -- $ENVARGS