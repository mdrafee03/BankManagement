using BankManagement.Application.Accounts;
using BankManagement.Application.Accounts.CreateAccount;
using BankManagement.Application.Accounts.GetAllAccounts;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BankManagement.Api.Endpoints.AccountEndpoints;

public static class AccountEndpoints
{
    public static void MapAccountEndpoints(this IEndpointRouteBuilder app)
    {
        var account = app.MapGroup("accounts");

        account.MapGet("/", GetAllAccounts);
        account.MapPost("/", CreateAccount);
    }

    private static async Task<Results<Created, BadRequest<string>>> CreateAccount(ISender sender,
        [FromBody] CreateAccountCommand command)
    {
        try
        {
            await sender.Send(command);
            return TypedResults.Created("Account created successfully.");
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }

    private static async Task<Results<Ok<List<AccountResponse>>, BadRequest<string>>> GetAllAccounts(ISender sender)
    {
        try
        {
            var result = await sender.Send(new GetAllAccountsQuery());
            return TypedResults.Ok(result);
        }
        catch (Exception ex)
        {
            return TypedResults.BadRequest(ex.Message);
        }
    }
}