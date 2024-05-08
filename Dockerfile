# add busybox to pull sh binary from later
FROM busybox:1.35.0 as busybox
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source

# install dotnet AOT dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    clang zlib1g-dev

COPY . .
RUN dotnet publish -r linux-x64 -c Release -o /app CloseAsteroids.csproj

FROM mcr.microsoft.com/dotnet/nightly/runtime-deps:8.0-jammy-chiseled-aot
# get sh from busybox
COPY --from=busybox /bin/sh /bin/sh
WORKDIR /app
COPY --from=build /app .
ENV ASTEROIDARGS="--date-min=2099-12-25 --date-max=2100-01-01 --dist-max=0.2 --body=Mercury"
# use sh entrypoint instead of exec form because exec form cannot expand ENV args
SHELL ["/bin/sh", "-c"]
ENTRYPOINT /app/CloseAsteroids $ASTEROIDARGS