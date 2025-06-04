using BankManagement.Application.Abstractions;
using IbanNet.Registry;

namespace BankManagement.Infrastructure.Services.BanService;

internal class BanService : IBanService
{
    public string GenerateIBan(string countryCode)
    {
        var generator = new IbanGenerator();
        var iban = generator.Generate(countryCode);
        return iban.ToString();
    }
}