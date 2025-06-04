using BankManagement.Application.Abstractions;
using BankManagement.Domain.Abstraction;
using BankManagement.Domain.Account;
using BankManagement.Infrastructure.Repositories;
using BankManagement.Infrastructure.Services.BanService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BankManagement.Infrastructure;

public static class ServiceExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddPersistent(services, configuration);
        AddBanService(services);
    }

    private static void AddPersistent(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAccountRepository, AccountRepository>();
    }

    private static void AddBanService(IServiceCollection services)
    {
        services.AddSingleton<IBanService, BanService>();
    }
}