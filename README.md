# AirAstanaTask

This is a project to manage flights and retrieve data about flights.

---

## Set up

Ensure you have the following installed on your system:

1. **.NET SDK** (6.0) - [Download .NET](https://dotnet.microsoft.com/download)
2. **SQL Server** - Local or remote instance.
3. **Visual Studio** or any text editor/IDE (optional).

---

## Getting Started

### 1. Clone the Repository
Clone the project to your local machine:
```bash
git clone https://github.com/yescorp/AirAstanaTask
cd AirAstanaTask
```

### 2. Update Database Configuration

1. Open `appsettings.json` and update the `ConnectionStrings` section:

   ```json
   "ConnectionStrings": {
       "AirAstanaFlights": "Server=YOUR_SERVER_NAME;Database=YOUR_DATABASE_NAME;User Id=YOUR_USERNAME;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
   }
   ```

2. Replace `YOUR_SERVER_NAME`, `YOUR_DATABASE_NAME`, `YOUR_USERNAME`, and `YOUR_PASSWORD` with your SQL Server credentials.

### 3. Apply Database Migrations

Run the following commands to create and apply database migrations:

```bash
# Ensure the EF Core tools are installed
dotnet tool install --global dotnet-ef

# Apply the migration to the database
dotnet ef database update -s Presentation -p Infrastructure
```

### 4. Run the Application

Use the following command to start the application:

```bash
dotnet run
```

The API and API documentation will be available at https://localhost:7231/swagger/index.html

---

## Logging with Serilog

You can view log files in the `/logs` folder. In this project Requests, Exceptions and Database changes are logged.

---

## License
This project is licensed under the MIT License. See the LICENSE file for details.
