# Eco Powerhub

## Project Overview
Eco Powerhub is a backend application designed to provide energy solutions and recommendations based on user input regarding their electricity usage and property specifications. The system calculates optimal solar panel configurations and recommends energy packages tailored to users' needs.

## Features
- User property management and analysis
- Solar energy package recommendations based on:
  - Electricity usage patterns
  - Available surface area
  - Monthly electricity costs
  - Peak power requirements
- Dynamic calculation of required solar panels and batteries
- Cost estimation for complete solar solutions
- Integration with a database for storing user and package data

## Technologies Used
- C# (.NET 8.0)
- ASP.NET Core Web API
- Entity Framework Core for database operations
- AutoMapper for object mapping
- PostgresSQL Server Database
- Docker support for containerization
- Kubernetes for orchestration
- Jenkins for CI/CD

## Project Structure
- **Controllers**: REST API endpoints for handling client requests
- **Data**: Database context and configurations
- **DTO**: Data Transfer Objects for:
  - User Properties
  - Package Recommendations
  - Response Objects
- **Helpers**: Utility classes and helper methods
- **Models**: Entity models for:
  - User Properties
  - Solar Packages
  - System Components
- **Repositories**: 
  - Implementation of repository pattern
  - Generic repositories for common operations
  - Specialized services like PropertyRepository for solar calculations
- **AutoMapper**: Custom mapping profiles
- **UOW**: Unit of Work pattern implementation

## Key Features Implementation
- Solar calculation algorithms for:
  - Daily energy production estimation
  - Required panel count calculation
  - Battery capacity determination
  - Cost estimation
- Comprehensive validation of user inputs
- Error handling and detailed response messages
- Scalable architecture using repository pattern

## Setup Instructions
1. Clone the repository
2. Navigate to the `Back-end` directory
3. Install dependencies:
```bash
dotnet restore
```
4. Configure the database:
   - Update connection string in `appsettings.json`
   - Run migrations:
```bash
dotnet ef database update
```
5. Start the application:
```bash
dotnet run
```

## Docker Support
Build the container:
```bash
docker build -t ecopowerhub .
```

Run using docker-compose:
```bash
docker-compose up
```

## API Documentation
Once running, access the Swagger documentation at:
- Development: https://localhost:5001/swagger

## Environment Requirements
- .NET 8.0 SDK or later
- PostgreSQL Database
- Docker (optional)
- Kubernetes (optional)

## Contributing
1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a pull request

## License
This project is licensed under the MIT License.
