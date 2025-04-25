# Use official .NET SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Use runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app
COPY --from=build /app/out .

# Expose port (Render uses 10000 internally, but we’ll use 8080 for app)
EXPOSE 8080

ENTRYPOINT ["dotnet", "InternManagementAPI.dll"]
