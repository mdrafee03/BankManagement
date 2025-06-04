using BankManagement.Application.Abstractions;
using BankManagement.Domain.Abstraction;
using BankManagement.Domain.Account;
using MediatR;

namespace BankManagement.Application.Accounts.CreateAccount;

internal class CreateAccountCommandHandler(
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork,
    IBanService banService)
    : IRequestHandler<CreateAccountCommand, Unit>
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IBanService _banService = banService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var iban = _banService.GenerateIBan("RO");

        var account = Account.Create(
            firstName: request.FirstName,
            lastName: request.LastName,
            iBan: iban
        );

        _accountRepository.Add(account);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}