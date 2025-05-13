# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy .csproj and restore
COPY ./src/parkway.api/parkway.api.csproj ./parkway.api/
RUN dotnet restore ./parkway.api/parkway.api.csproj

# Copy the rest of the app and publish
COPY ./src/parkway.api/ ./parkway.api/
RUN dotnet publish ./parkway.api/parkway.api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "parkway.api.dll"]
