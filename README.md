# OpenBanking API Service

This project is an implementation of a banking system API using C# and ASP.NET Core. It provides functionalities for user registration, authentication, account management, and transactions.

## Features

- **User Registration:** Users can register with the system providing necessary details.
- **Login:** Registered users can log in to their accounts securely.
- **JWT Authentication:** JSON Web Token (JWT) mechanism is used for secure authentication and authorization.
- **Email Confirmation:** Upon registration, users receive an email confirmation link for account activation.
- **Password Reset:** Users can request a password reset through email verification.
- **Two-Factor Authentication (2FA):** Optional two-factor authentication is available for enhanced security.
- **Account Management:** Users can create bank accounts, perform deposits, withdrawals, transfers, and view transaction history.
- **Paging, Filtering, Sorting, Searching:** Implemented to enhance API performance.
- **Generic Repository Pattern:** Implemented for efficient data access and manipulation.

## Technologies Used

- **ASP.NET Core:** Framework for building APIs using C#.
- **SQL Server:** Used as the database management system.
- **ASP.NET Core Identity:** Provides user authentication and management functionalities.
- **Gmail Service:** Integrated for sending email confirmations, password reset links, and OTP codes.

## Prerequisites

- .NET 8 SDK must be installed on your system.
- SQL Server for database storage.

## Running the Project Locally

1. **Clone the repository** to your local machine:
   ```bash
   git clone https://github.com/Promise30/OpenBanking-API-Service-With-AspNetCore
  
2. **Navigate to the project directory**:
    ```bash
   cd OpenBanking_API_Service

3. **Update Database Connection String**:
    Navigate to the appsettings.json file in the project directory.
    Modify the "DefaultConnection" connection string to point to your local SQL Server instance.
    Run Migrations:
    ```bash
    dotnet ef database update

4. **Run the Application**:
    ```bash
    dotnet run

5. **The API service should now be running locally.**
