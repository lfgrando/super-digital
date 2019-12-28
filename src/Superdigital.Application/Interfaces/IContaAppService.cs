using Superdigital.Domain.Entities;
using Superdigital.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Superdigital.Application.Interfaces
{
    public interface IContaAppService : IAppServiceBase<Conta>
    {
        Task<Result<string>> GenerateAccountNumberAsync();
        Task<Result<bool>> ValidateDuplicateAccountNumberAsync(ContaRequest contaRequest);
        Task<Result<bool>> UpdateAccountAsync(ContaRequest conta);
    }
}
