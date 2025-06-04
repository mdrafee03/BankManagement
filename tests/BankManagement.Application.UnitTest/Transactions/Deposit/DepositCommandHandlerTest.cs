using System.Linq.Expressions;
using BankManagement.Application.Transactions.Deposit;
using BankManagement.Domain.Abstraction;
using BankManagement.Domain.Account;
using NSubstitute;

namespace BankManagement.Application.UnitTest.Transactions.Deposit;

public class DepositCommandHandlerTest
{
    private readonly IAccountRepository _accountRepository;
    private readonly DepositCommandHandler _handler;
    private readonly IUnitOfWork _unitOfWork;

    public DepositCommandHandlerTest()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new DepositCommandHandler(_accountRepository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_DepositsAmountMinusFeeAndSaves_WhenAccountExists()
    {
        var account = Substitute.For<Account>();
        var iban = "IBAN123";
        var amount = 100m;
        _accountRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Account, bool>>>())
            .Returns(account);

        var command = new DepositCommand(iban, amount);

        await _handler.Handle(command, CancellationToken.None);

        account.Received(1).Deposit(99m);
        _accountRepository.Received(1).Update(account);

        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ThrowsInvalidOperationException_WhenAccountDoesNotExist()
    {
        _accountRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Account, bool>>>())
            .Returns((Account?)null);

        var command = new DepositCommand("IBAN_NOT_FOUND", 50m);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
    }
}