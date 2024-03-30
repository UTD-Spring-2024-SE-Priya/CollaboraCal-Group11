# CollaboraCal

CollaboraCal is a webhosted calendar application for task tracking, planning, and organization. CollaboraCal allows for users to share, edit, and view calendars with friends.

## Included in this repository:

1. Frontend (CollaboraCal/frontend/)

Frontend website built with React and Node.js

2. Backend (CollaboraCal/backend)

Backend web-api built with ASP.NET/C# and a database powered by SQLite.

3. Testing (CollaboraCal/testing)

Unit testing suite using MSTest.
Run tests with "dotnet test"

## Database Migration Warning:

In order for database operations to succeed and not result in an exception:

1. A folder path existing to the directory described in the connection string found in 'application.json' or 'application.Development.json'
2. Database migrations **must** be applied. For information about how to apply database migrations see heading **Applying Database Migrations**

## Applying Database Migrations:

1. The dotnet entity framework tool must be acquired.

If the command "dotnet ef" does not exist, run the command:
> dotnet tool install dotnet-ef --global

2. Apply database migrations:

Navigate to the backend project directory (CollaboraCal/backend/)
Run the database update command:
> dotnet ef database update

## Frontend

To launch the frontend use the "npm start" command in the frontend directory.
In order to launch, the packages must be installed. To install the packages run the "npm install" command.

## Backend

Use "dotnet run" in the backend directory to build and run.
"dotnet build" in the base directory will build all .NET projects in the solution.