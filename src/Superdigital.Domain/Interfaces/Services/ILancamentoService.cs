using Superdigital.Domain.Entities;
using Superdigital.Domain.ViewModels.Lancamento;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Superdigital.Domain.Interfaces.Services
{
    public interface ILancamentoService : IServiceBase<Lancamento>
    {
        Task<Result<decimal>> DepositAsync(LancamentoRequest lancamentoRequest);
        Task<Result<decimal>> TransferAsync(LancamentoRequest lancamentoRequest);
        Task<Result<decimal>> WithdrawAsync(LancamentoRequest lancamentoRequest);
    }
}
