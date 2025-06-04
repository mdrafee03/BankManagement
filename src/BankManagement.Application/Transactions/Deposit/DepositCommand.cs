using MediatR;

namespace BankManagement.Application.Transactions.Deposit;

public record DepositCommand(string IBan, decimal Amount): IRequest<Unit>;