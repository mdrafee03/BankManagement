using BankManagement.Domain.Abstraction;
using BankManagement.Domain.Account;
using BankManagement.Domain.Account.AccountTransferService;
using MediatR;

namespace BankManagement.Application.Transactions.Transfer;

public class TransferCommandHandler(
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork,
    IAccountService accountService)
    : IRequestHandler<TransferCommand, Unit>
{
    private readonly IAccountRepository _accountRepository = accountRepository;
    private readonly IAccountService _accountService = accountService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var accountFrom = await _accountRepository.GetFirstOrDefaultAsync(ac => ac.IBan == request.FromAccountIBan);
        if (accountFrom == null) throw new InvalidOperationException("Source account not found with the provided IBan");

        if (accountFrom.Balance < request.Amount)
            throw new InvalidOperationException("Insufficient funds in the source account");

        var accountTo = await _accountRepository.GetFirstOrDefaultAsync(ac => ac.IBan == request.ToAccountIBan);
        if (accountTo == null)
            throw new InvalidOperationException("Destination account not found with the provided IBan");

        if (accountFrom.IBan == accountTo.IBan)
            throw new InvalidOperationException("Cannot transfer to the same account");

        _accountService.Transfer(accountFrom, accountTo, request.Amount);

        _accountRepository.Update(accountFrom);
        _accountRepository.Update(accountTo);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}