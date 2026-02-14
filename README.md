# Gym Periodisation

A **gym workout tracker** built with **.NET 10**, **SQL Server**, and **Entity Framework Core**.  
This project allows users to track workouts, exercises, muscles, and workout sets. It also includes a **console-based seeder** to populate the database with initial data.

A backend API with Database is to be configured.
Afterwards, the frontend will be developed.

The end goal is to have a workout tracker which implements the standard features you would expect:
* Workout logger
  * Start a workout
  * Select exercises
  * Add sets and reps
* View past workouts
* View specific exercise information
  * Global and personal
  
As well as:
* Plateu indicator
* Exercise suggestions/predictions
* Exercises sets and reps prediction

---

## Project Structure
```
backend/
├─ GymPeriodisation.slnx <- Solution file
├─ src/
│ ├─ GymPeriodisation.Api <- API project
│ ├─ GymPeriodisation.Application <- Application layer (business logic)
│ ├─ GymPeriodisation.Domain <- Domain entities
│ ├─ GymPeriodisation.Infrastructure <- EF Core DbContext and persistence
│ └─ GymPeriodisation.Seeder <- Console project to seed the database
└─ tests/
└─ GymPeriodisation.Tests <- Unit tests
```
---

## Features

- **Users**: Track multiple users (local version, no full auth yet)
- **Exercises**: Define exercises linked to multiple muscles (many-to-many)
- **Muscles**: Predefined muscle groups
- **Workout Sets**: Log sets, reps, and weights
- **Seeder Project**: Populate Muscles, Exercises, and Workouts from JSON files
- **API**: Provides endpoints for CRUD operations on all entities
- **Swagger/OpenAPI** support for testing endpoints

---

## Setup Instructions

### 1. Clone the repository

```bash
git clone <your-repo-url>
cd gym-periodisation/backend
```

### 2. Configure SQL Server
* Ensure SQL Server is running locally.
* Update the connection string in GymPeriodisation.Api/appsettings.json:

```
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=GymPeriodisation;Trusted_Connection=True;"
}
```

### 3. Build the solution
```dotnet build GymPeriodisation.slnx```

### 4. Run migrations
```dotnet build GymPeriodisation.slnx```

### 5. Seed the database
```dotnet run --project src/GymPeriodisation.Seeder/GymPeriodisation.Seeder.csproj```
* The seeder reads JSON files in src/GymPeriodisation.Seeder/Data/
* Adds Muscles, Exercises, Workouts, etc., and is safe to run multiple times.

### 6. Run the API
```dotnet run --project src/GymPeriodisation.Api/GymPeriodisation.Api.csproj```
