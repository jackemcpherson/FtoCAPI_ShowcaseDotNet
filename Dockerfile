# Use the official .NET SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Set the working directory in the container
WORKDIR /app

# Copy the project file to the working directory
COPY *.csproj ./

# Restore the project dependencies with verbose output and retry
RUN dotnet nuget list source && \
    dotnet restore --verbosity detailed || \
    (echo "Retrying restore with HTTP source..." && \
     dotnet nuget disable source "https://api.nuget.org/v3/index.json" && \
     dotnet nuget enable source "http://api.nuget.org/v3/index.json" && \
     dotnet restore --verbosity detailed)

# Copy the entire project to the working directory
COPY . ./

# Build the application in release mode
RUN dotnet publish -c Release -o out

# Use the official ASP.NET Core runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory in the container
WORKDIR /app

# Copy the published output from the build stage to the working directory
COPY --from=build /app/out .

# Expose port 8000 for the application
EXPOSE 8000

# Set the entry point for the container
ENTRYPOINT ["dotnet", "FtoCAPI_ShowcaseDotNet.dll"]