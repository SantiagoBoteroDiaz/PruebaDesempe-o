Sports Complex Reservation System

Web system developed in **ASP.NET Core MVC** with **C#** for managing users, sports spaces, and reservations at a sports complex. Replaces manual processes by eliminating scheduling conflicts, double bookings, and data loss.
 
---

## Table of Contents

- [Prerequisites](#prerequisites)
- [Technologies](#technologies)
- [Project Structure](#project-structure)
- [Setup](#setup)
- [Migrations and Database](#migrations-and-database)
- [Running the Project](#running-the-project)
- [Features](#features)
- [Business Rules](#business-rules)
---

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [MySQL 8+](https://dev.mysql.com/downloads/)
- [Git](https://git-scm.com/)
- Recommended IDE: [JetBrains Rider](https://www.jetbrains.com/rider/) or [Visual Studio 2022](https://visualstudio.microsoft.com/)
---

## Technologies

| Technology | Version | Purpose |
|---|---|---|
| ASP.NET Core MVC | 10.0 | Web framework |
| Entity Framework Core | 9.0 | ORM |
| Pomelo.EntityFrameworkCore.MySql | 9.0 | MySQL connector |
| MailKit / MimeKit | 4.9 | Email sending |
| LINQ | - | Data queries |
| Materialize CSS | - | UI styles |
 
---

## Project Structure

```
GestionDeEspacios/
├── Controllers/         # MVC Controllers
├── Data/                # DbContext
├── Enums/               # ReservationStatus, SportSpacesType
├── Interfaces/          # Service contracts
├── Migrations/          # EF Core migrations
├── Models/              # Entities: User, SportSpace, Reservation
├── ModelView/           # ViewModels
├── Response/            # SystemResponse<T> generic wrapper
├── Services/            # Business logic
├── Settings/            # EmailSettings
├── Views/               # Razor views (.cshtml)
├── wwwroot/             # Static files
├── appsettings.json
└── Program.cs
```
 
---

## Setup

**1. Clone the repository**

```bash
git clone https://github.com/SantiagoBoteroDiaz/PruebaDesempe-o.git
cd PruebaDesempe-o
```

**2. Install NuGet packages**

```bash
dotnet add package Microsoft.EntityFrameworkCore --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 9.0.0
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 9.0.0
dotnet add package MailKit --version 4.9.0
dotnet add package MimeKit --version 4.9.0
```

**3. Configure the connection string**

Edit `appsettings.Development.json` with your MySQL credentials:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SistemaReservas;User=root;Password=yourpassword;"
  },
  "EmailSettings": {
    "Host": "smtp.gmail.com",
    "Port": 587,
    "Email": "youremail@gmail.com",
    "Password": "your_app_password",
    "NombreRemitente": "Sports Complex"
  }
}
```
**4. Configure Gmail App Password**

To enable email sending with Gmail:
1. Go to [myaccount.google.com](https://myaccount.google.com)
2. Security → 2-Step Verification → App Passwords
3. Generate a key and place it in `EmailSettings.Password`
---

## Migrations and Database

```bash
# Install EF Core CLI tool (first time only)
dotnet tool install --global dotnet-ef --version 9.0.0
 
# Apply migrations and create the database
dotnet ef database update
```

To recreate the database from scratch:

```bash
dotnet ef database drop
dotnet ef migrations remove
dotnet ef migrations add InitialCreate
dotnet ef database update
```
 
---

## Running the Project

```bash
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.
 
---

## Features

### User Management
- Register, edit, and list users
- Unique document, phone, and email validation
- Document and phone only allow numbers
### Sports Space Management
- Register, edit, and list sports spaces
- Filter spaces by type (Football, Basketball, Pool, Tennis, etc.)
- Duplicate validation by name and type
### Reservation Management
- Create, edit, and cancel reservations
- List reservations by user and by sports space
- States: `Scheduled`, `Cancelled`, `Finished`, `NoShow`
### Notifications
- Automatic email sent when a reservation is created
---

## Business Rules

- A sports space cannot have two reservations in overlapping time slots
- A user cannot have two reservations in the same time range
- End time must be greater than start time
- Reservations cannot be created on past dates or times
- Duplicate documents, phones, or emails are not allowed
---

## Diagrams

Class and use case diagrams are located in the `/docs` folder of the repository.
 
---

## Author

Santiago Botero Diaz - Coder Riwi
 
