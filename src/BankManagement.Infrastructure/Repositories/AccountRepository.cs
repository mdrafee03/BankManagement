using System.Linq.Expressions;
using BankManagement.Domain.Account;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.Infrastructure.Repositories;

internal class AccountRepository(AppDbContext appDbContext) : IAccountRepository
{
    public async Task<List<Account>?> GetAllAsync()
    {
        return await appDbContext.Accounts.ToListAsync();
    }

    public Task<Account?> GetFirstOrDefaultAsync(Expression<Func<Account, bool>> where)
    {
        var result = appDbContext.Accounts.FirstOrDefaultAsync(where);
        return result;
    }

    public void Add(Account account)
    {
        appDbContext.Accounts.Add(account);
    }

    public void Update(Account account)
    {
        appDbContext.Accounts.Update(account);
    }
}