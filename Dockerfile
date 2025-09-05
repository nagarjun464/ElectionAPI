# Use .NET 8 runtime as base
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish -c Release -o /app/out

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Election.API.dll"]
