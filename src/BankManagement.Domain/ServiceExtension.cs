using BankManagement.Domain.Account.AccountTransferService;
using Microsoft.Extensions.DependencyInjection;

namespace BankManagement.Domain;

public static class ServiceExtension
{
    public static void AddDomain(this IServiceCollection services)
    {
        services.AddSingleton<IAccountService, AccountService>();
    }
}