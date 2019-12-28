using Superdigital.Domain.Entities;
using Superdigital.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Superdigital.Domain.Interfaces.Services
{
    public interface IContaService : IServiceBase<Conta>
    {
        Task<Result<string>> GenerateAccountNumberAsync();
        Task<Result<bool>> UpdateBalanceAsync(string id, decimal valor);
        Task<Result<bool>> UpdateAccountAsync(ContaRequest conta);
    }
}
