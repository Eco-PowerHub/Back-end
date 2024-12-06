# Base image SDK version-8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the Dependencie's files inside the image 
COPY *.csproj .
COPY *.sln .

# Install all dependencies
RUN dotnet restore

# Copy all project's files
COPY . .

# Publish the ASP.NET app 
RUN dotnet publish -c Release -o /app/build

# Run Time image 
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app/build .

# Expose to port 80
EXPOSE 80

# Run our ASP.NET app
ENTRYPOINT [ "dotnet", "EcoPowerHub.dll" ]




