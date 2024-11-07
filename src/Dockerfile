FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /src
# Skopiuj pliki .csproj i przywróć zależności
COPY WebApi/WebApi.csproj WebApi/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
RUN dotnet restore WebApi/WebApi.csproj
RUN dotnet restore Application/Application.csproj
RUN dotnet restore Domain/Domain.csproj
RUN dotnet restore Infrastructure/Infrastructure.csproj

# Skopiuj resztę plików i zbuduj aplikację
COPY . ./
RUN dotnet publish WebApi/WebApi.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build-env /src/out .

# Skopiuj pliki DLL
COPY --from=build-env /src/WebApi/bin/Release/net8.0/WebApi.dll .
COPY --from=build-env /src/Application/bin/Release/net8.0/Application.dll .
COPY --from=build-env /src/Domain/bin/Release/net8.0/Domain.dll .
COPY --from=build-env /src/Infrastructure/bin/Release/net8.0/Infrastructure.dll .

EXPOSE 5000

# Ustaw punkt wejścia dla kontenera
ENTRYPOINT ["dotnet", "WebApi.dll"]