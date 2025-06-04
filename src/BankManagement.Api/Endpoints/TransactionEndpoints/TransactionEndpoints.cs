using BankManagement.Application.Transactions.Deposit;
using BankManagement.Application.Transactions.Transfer;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankManagement.Api.Endpoints.TransactionEndpoints;

public static class TransactionEndpoints
{
    public static void MapTransferEndpoints(this IEndpointRouteBuilder app)
    {
        var transfer = app.MapGroup("transactions");

        transfer.MapPost("/deposit", Deposit);
        transfer.MapPost("/transfer", Transfer);
    }

    private static async Task<Results<Ok, BadRequest<string>>> Deposit(ISender sender,
        [FromBody] DepositCommand command)
    {
        try
        {
            await sender.Send(command);
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static async Task<Results<Ok, BadRequest<string>>> Transfer(ISender sender,
        [FromBody] TransferCommand command)
    {
        try
        {
            await sender.Send(command);
            return TypedResults.Ok();
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}