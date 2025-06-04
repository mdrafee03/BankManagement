using FluentValidation;

namespace BankManagement.Application.Transactions.Deposit;

public class DepositCommandValidator : AbstractValidator<DepositCommand>
{
    public DepositCommandValidator()
    {
        RuleFor(x => x.IBan)
            .NotEmpty()
            .WithMessage("Account IBan is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Deposit amount must be greater than zero.");
    }
}