using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagement.Application.Accounts.GetAllAccounts
{
    public record GetAllAccountsQuery: IRequest<List<AccountResponse>>;
}
