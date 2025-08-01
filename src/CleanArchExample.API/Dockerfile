# ----------- Build stage -----------
FROM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
WORKDIR /app

# Copy solution and restore as distinct layers
COPY CleanArchExample.sln ./
COPY src/ ./src/
COPY tests/ ./tests/

WORKDIR /app/src/CleanArchExample.API
RUN dotnet restore ../../CleanArchExample.sln

# Build
RUN dotnet publish -c Release -o /app/out --no-restore

# ----------- Runtime stage -----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS final
WORKDIR /app
COPY --from=build /app/out ./

# (Optional) set environment variable
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CleanArchExample.API.dll"]
