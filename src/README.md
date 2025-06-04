# BankManagement

A modular, testable banking management system built with ASP.NET Core, supporting account management and transactions.

## Features

- **Account Management**: Create and list bank accounts.
- **Transactions**: Deposit, transfer funds between accounts.
- **Domain-Driven Design**: Clear separation of concerns across Domain, Application, and Infrastructure layers.
- **Extensible API**: RESTful endpoints with OpenAPI/Swagger documentation.
- **Unit Tested**: Comprehensive tests using xUnit and NSubstitute.

## Project Structure

- `src/BankManagement.Api` – ASP.NET Core Web API (entry point)
- `src/BankManagement.Application` – Application logic (CQRS, handlers)
- `src/BankManagement.Domain` – Domain models and services
- `src/BankManagement.Infrastructure` – Data access and persistence
- `tests/BankManagement.Application.UnitTest` – Unit tests

## Getting Started

1. **Clone the repository**
   ```sh
   git clone https://github.com/mdrafee03/BankManagement.git
   cd BankManagement
   ```

2. **Run the API**
   ```sh
   dotnet run --project src/BankManagement.Api
   ```

3. **Explore the API**
    - Swagger UI: [https://localhost:7031/swagger](https://localhost:7031/swagger)

## Testing

Run all unit tests:
```sh
dotnet test
```

## Technologies

- .NET 9 / ASP.NET Core
- MediatR (CQRS)
- xUnit, NSubstitute (testing)
- Swagger/OpenAPI
- Entity Framework Core (with SQLite for simplicity)
- FluentValidation (input validation)

## Contributing

Contributions are welcome! Please open issues or submit pull requests.