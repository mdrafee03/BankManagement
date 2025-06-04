namespace BankManagement.Domain.Account.AccountTransferService;

public interface IAccountService
{
    void Transfer(Account from, Account to, decimal amount);
}