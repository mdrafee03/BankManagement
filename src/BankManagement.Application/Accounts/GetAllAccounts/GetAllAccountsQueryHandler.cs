using BankManagement.Domain.Account;
using MediatR;

namespace BankManagement.Application.Accounts.GetAllAccounts;

internal class GetAllAccountsQueryHandler(IAccountRepository accountRepository)
    : IRequestHandler<GetAllAccountsQuery, List<AccountResponse>>
{
    private readonly IAccountRepository _accountRepository = accountRepository;

    public async Task<List<AccountResponse>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        var result = await _accountRepository.GetAllAsync();
        if (result == null) return [];

        var accounts = result.Select(account =>
            new AccountResponse
            {
                IBan = account.IBan,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Balance = account.Balance
            }
        ).ToList();
        return accounts;
    }
}