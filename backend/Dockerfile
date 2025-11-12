# ===============================
# STAGE 1: Build and Test
# ===============================

# Use the official .NET 9 SDK image as build environment.
# This image includes all necessary tools for building, restoring,
# and testing .NET applications.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Set the working directory inside the container.
WORKDIR /src

# Copy the entire backend source code into the container.
# This includes the solution file and all related project folders.
COPY . .

# Restore all NuGet dependencies defined in the solution.
# This step downloads all required packages to the container.
RUN dotnet restore DeliFHery.sln

# Run all unit tests defined in the Data.Tests project.
# Using '--no-restore' skips restoring packages again since it was already done.
# '--verbosity normal' provides detailed test output for debugging.
RUN dotnet test DeliFHery.Tests/DeliFHery.Tests.csproj --no-restore --verbosity normal

# Publish the API project in Release mode to a dedicated output directory.
# '--no-restore' reuses the previous restore step to save time.
# The build output will be placed in '/app/publish' for the next stage.
RUN dotnet publish DeliFHery.Api/DeliFHery.Api.csproj -c Release -o /app/publish --no-restore


# ===============================
# STAGE 2: Production Runtime
# ===============================

# Use a lightweight ASP.NET Core runtime image for production.
# This image only contains the necessary components to run .NET applications,
# making it much smaller than the SDK image.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

# Set the working directory where the published files will reside.
WORKDIR /app

# Copy the published output from the previous build stage into this runtime image.
COPY --from=build /app/publish .

# Define environment variable to configure ASP.NET Core to listen on port 8080.
ENV ASPNETCORE_URLS=http://+:8080

# Expose port 8080 to allow traffic from outside the container.
EXPOSE 8080

# Define the entry point to start the .NET application when the container runs.
ENTRYPOINT ["dotnet", "DeliFHery.Api.dll"]

