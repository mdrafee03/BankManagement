using System.Linq.Expressions;

namespace BankManagement.Domain.Account;

public interface IAccountRepository
{
    Task<Account?> GetFirstOrDefaultAsync(Expression<Func<Account, bool>> where);
    Task<List<Account>?> GetAllAsync();
    void Add(Account account);

    void Update(Account account);
}