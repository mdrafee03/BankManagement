using System.Linq.Expressions;
using BankManagement.Application.Transactions.Transfer;
using BankManagement.Domain.Abstraction;
using BankManagement.Domain.Account;
using BankManagement.Domain.Account.AccountTransferService;
using NSubstitute;

namespace BankManagement.Application.UnitTest.Transactions.Transfer;

public class TransferCommandHandlerTest
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountService _accountService;
    private readonly TransferCommandHandler _handler;
    private readonly IUnitOfWork _unitOfWork;

    public TransferCommandHandlerTest()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _accountService = Substitute.For<IAccountService>();
        _handler = new TransferCommandHandler(_accountRepository, _unitOfWork, _accountService);
    }

    [Fact]
    public async Task Handle_TransfersAmountAndSaves_WhenAllConditionsAreMet()
    {
        var fromAccount = new Account
        {
            IBan = "FROM_IBAN",
            FirstName = "John",
            LastName = "Doe",
            Balance = 200m
        };

        var toAccount = new Account
        {
            IBan = "TO_IBAN",
            FirstName = "Jane",
            LastName = "Smith",
            Balance = 50m
        };

        _accountRepository
            .GetFirstOrDefaultAsync(Arg.Is<Expression<Func<Account, bool>>>(expr =>
                expr.Compile()(fromAccount)))
            .Returns(fromAccount);

        _accountRepository.GetFirstOrDefaultAsync(Arg.Is<Expression<Func<Account, bool>>>(expr =>
                expr.Compile()(toAccount)))
            .Returns(toAccount);

        var command = new TransferCommand(fromAccount.IBan, toAccount.IBan, 100m);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _accountService.Received(1).Transfer(fromAccount, toAccount, 100m);
        _accountRepository.Received(1).Update(fromAccount);
        _accountRepository.Received(1).Update(toAccount);

        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_Throws_WhenSourceAccountNotFound()
    {
        _accountRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Account, bool>>>())
            .Returns((Account?)null);

        var command = new TransferCommand("FROM_IBAN", "TO_IBAN", 100m);

        var result =
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Source account not found with the provided IBan", result.Message);
    }

    [Fact]
    public async Task Handle_Throws_WhenInsufficientFunds()
    {
        var fromAccount = Substitute.For<Account>();
        fromAccount.IBan = "FROM_IBAN";
        fromAccount.Balance = 50m;

        _accountRepository
            .GetFirstOrDefaultAsync(Arg.Is<Expression<Func<Account, bool>>>(exp => exp.Compile()(fromAccount)))
            .Returns(fromAccount);

        var toAccount = Substitute.For<Account>();
        toAccount.IBan = "TO_IBAN";
        toAccount.Balance = 100m;

        _accountRepository
            .GetFirstOrDefaultAsync(Arg.Is<Expression<Func<Account, bool>>>(exp => exp.Compile()(toAccount)))
            .Returns(toAccount);

        var command = new TransferCommand("FROM_IBAN", "TO_IBAN", 100m);

        var result =
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Insufficient funds in the source account", result.Message);
    }

    [Fact]
    public async Task Handle_Throws_WhenDestinationAccountNotFound()
    {
        var fromAccount = Substitute.For<Account>();
        fromAccount.IBan = "FROM_IBAN";
        fromAccount.Balance = 200m;

        _accountRepository
            .GetFirstOrDefaultAsync(Arg.Is<Expression<Func<Account, bool>>>(exp => exp.Compile()(fromAccount)))
            .Returns(fromAccount);

        _accountRepository
            .GetFirstOrDefaultAsync(Arg.Is<Expression<Func<Account, bool>>>(exp => exp.Compile()(null!)))
            .Returns((Account?)null);

        var command = new TransferCommand("FROM_IBAN", "TO_IBAN", 100m);


        var result =
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        Assert.Equal("Destination account not found with the provided IBan", result.Message);
    }

    [Fact]
    public async Task Handle_Throws_WhenTransferringToSameAccount()
    {
        var account = Substitute.For<Account>();
        account.IBan = "SAME_IBAN";
        account.Balance = 200m;

        _accountRepository.GetFirstOrDefaultAsync(Arg.Any<Expression<Func<Account, bool>>>())
            .Returns(account);

        var command = new TransferCommand("SAME_IBAN", "SAME_IBAN", 100m);

        var result = await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, default));
        Assert.Equal("Cannot transfer to the same account", result.Message);
    }
}