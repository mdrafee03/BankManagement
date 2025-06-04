using BankManagement.Application.Abstractions;
using BankManagement.Application.Accounts.CreateAccount;
using BankManagement.Domain.Abstraction;
using BankManagement.Domain.Account;
using NSubstitute;

namespace BankManagement.Application.UnitTest.Accounts.CreateAccount;

public class CreateAccountCommandHandlerTest
{
    private readonly IAccountRepository _accountRepository;
    private readonly IBanService _banService;

    private readonly CreateAccountCommandHandler _handler;
    private readonly IUnitOfWork _unitOfWork;

    public CreateAccountCommandHandlerTest()
    {
        _accountRepository = Substitute.For<IAccountRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _banService = Substitute.For<IBanService>();

        _handler = new CreateAccountCommandHandler(_accountRepository, _unitOfWork, _banService);
    }

    [Fact]
    public async Task Handle_ShouldCreateAccountAndSaveChanges()
    {
        // Arrange
        const string fakeIban = "RO123456";
        var command = new CreateAccountCommand("John", "Doe");

        _banService.GenerateIBan("RO").Returns(fakeIban);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _accountRepository.Received(1).Add(Arg.Is<Account>(a =>
            a.FirstName == "John" &&
            a.LastName == "Doe" &&
            a.IBan == fakeIban
        ));
        await _unitOfWork.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}