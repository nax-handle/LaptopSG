# LaptopSG

A .NET-based web application for managing laptop inventory and sales.

## Project Overview

LaptopSG is a web application built using ASP.NET Core, designed to provide a comprehensive solution for laptop inventory management and sales operations.

## Project Structure

```
LaptopSG/
├── Admin/           # Admin-related components
├── Controllers/     # Application controllers
├── Models/         # Data models
├── Views/          # View templates
├── Middlewares/    # Custom middleware components
└── Properties/     # Project properties and configuration
```

## Prerequisites

- .NET 6.0 or later
- Visual Studio 2022 or later
- SQL Server (for database)

## Getting Started

1. Clone the repository:

   ```bash
   git clone [repository-url]
   ```

2. Open the solution in Visual Studio:

   - Open `LaptopSG_Sln.sln`

3. Restore NuGet packages:

   - Right-click on the solution in Solution Explorer
   - Select "Restore NuGet Packages"

4. Update the database connection string in `appsettings.json` with your SQL Server details

5. Build and run the application:
   - Press F5 or click the "Start" button in Visual Studio

## Features

- User authentication and authorization
- Admin dashboard
- Laptop inventory management
- Sales tracking
- User management

## Configuration

The application uses the following configuration files:

- `appsettings.json` - Main configuration file
- `appsettings.Development.json` - Development-specific settings

## Contributing

1. Fork the repository
2. Create a new branch for your feature
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

[Add your license information here]

## Contact

[Add your contact information here]
