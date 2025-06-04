namespace BankManagement.Domain.Account.AccountTransferService;

public class AccountService : IAccountService
{
    public void Transfer(Account from, Account to, decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Transfer amount must be greater than zero");

        from.Withdraw(amount);
        to.Deposit(amount);
    }
}