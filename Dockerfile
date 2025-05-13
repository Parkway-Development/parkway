# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ./src/Parkway.Api/*.csproj ./Parkway.Api/
RUN dotnet restore ./Parkway.Api/Parkway.Api.csproj
COPY ./src/ .
RUN dotnet publish ./Parkway.Api/Parkway.Api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Parkway.Api.dll"]
