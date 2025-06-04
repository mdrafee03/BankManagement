using BankManagement.Application.Accounts.GetAllAccounts;
using BankManagement.Domain.Account;
using NSubstitute;

namespace BankManagement.Application.UnitTest.Accounts.GetAllAccounts;

public class GetAllAccountsQueryHandlerTest
{
    [Fact]
    public async Task Handle_ReturnsMappedAccountResponses_WhenAccountsExist()
    {
        var accountRepository = Substitute.For<IAccountRepository>();
        var accounts = new List<Account>
        {
            new() { IBan = "IBAN1", FirstName = "Alice", LastName = "Smith", Balance = 100 },
            new() { IBan = "IBAN2", FirstName = "Bob", LastName = "Jones", Balance = 200 }
        };
        accountRepository.GetAllAsync().Returns(accounts);

        var handler = new GetAllAccountsQueryHandler(accountRepository);

        var result = await handler.Handle(new GetAllAccountsQuery(), default);

        Assert.Equal(2, result.Count);
        Assert.Contains(result,
            r => r is { IBan: "IBAN1", FirstName: "Alice", LastName: "Smith", Balance: 100 });
        Assert.Contains(result,
            r => r is { IBan: "IBAN2", FirstName: "Bob", LastName: "Jones", Balance: 200 });
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoAccountsExist()
    {
        var accountRepository = Substitute.For<IAccountRepository>();
        accountRepository.GetAllAsync().Returns(new List<Account>());

        var handler = new GetAllAccountsQueryHandler(accountRepository);

        var result = await handler.Handle(new GetAllAccountsQuery(), default);

        Assert.Empty(result);
    }
}