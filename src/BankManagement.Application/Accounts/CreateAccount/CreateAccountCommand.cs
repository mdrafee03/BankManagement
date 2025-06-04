using MediatR;

namespace BankManagement.Application.Accounts.CreateAccount;

public record CreateAccountCommand(string FirstName, string LastName) : IRequest<Unit>;