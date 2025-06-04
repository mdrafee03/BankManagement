namespace BankManagement.Application.Abstractions;

public interface IBanService
{
    public string GenerateIBan(string countryCode);
}