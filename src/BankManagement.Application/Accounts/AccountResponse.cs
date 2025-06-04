namespace BankManagement.Application.Accounts;

public record AccountResponse
{
    public required string IBan { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required decimal Balance { get; init; }
}