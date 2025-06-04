using BankManagement.Domain.Abstraction;
using BankManagement.Domain.Account;
using MediatR;

namespace BankManagement.Application.Transactions.Deposit;

internal class DepositCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<DepositCommand, Unit>
{
    private const decimal DepositFee = 1m;
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetFirstOrDefaultAsync(ac => ac.IBan == request.IBan);

        if (account == null) throw new InvalidOperationException("Account not found with the provided IBan");

        var depositAfterFee = request.Amount * (1 - DepositFee / 100);

        account.Deposit(depositAfterFee);

        _accountRepository.Update(account);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}