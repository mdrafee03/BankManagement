using MediatR;

namespace BankManagement.Application.Transactions.Transfer;

public record TransferCommand(string FromAccountIBan, string ToAccountIBan, decimal Amount) : IRequest<Unit>;