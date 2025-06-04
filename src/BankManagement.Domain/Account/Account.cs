namespace BankManagement.Domain.Account;

public class Account
{
    public Guid Id { get; set; }
    public required string IBan { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }

    public decimal Balance { get; set; }

    public static Account Create(string iBan, string firstName, string lastName)
    {
        return new Account
        {
            Id = Guid.CreateVersion7(),
            IBan = iBan,
            FirstName = firstName,
            LastName = lastName,
            Balance = 0
        };
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0) throw new InvalidOperationException("Withdrawal amount must be greater than zero.");

        if (amount > Balance) throw new InvalidOperationException("Insufficient funds for withdrawal.");

        Balance -= amount;
    }
}