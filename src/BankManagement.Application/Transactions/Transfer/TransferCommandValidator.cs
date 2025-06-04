using FluentValidation;

namespace BankManagement.Application.Transactions.Transfer;

public sealed class TransferCommandValidator : AbstractValidator<TransferCommand>
{
    public TransferCommandValidator()
    {
        RuleFor(x => x.FromAccountIBan)
            .NotEmpty()
            .WithMessage("From account IBan is required.");

        RuleFor(x => x.ToAccountIBan)
            .NotEmpty()
            .WithMessage("To account IBan is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Transfer amount must be greater than zero.");
    }
}