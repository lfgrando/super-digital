using Superdigital.Domain.Entities;
using Superdigital.Domain.ViewModels.Lancamento;
using System.Threading;
using System.Threading.Tasks;

namespace Superdigital.Application.Interfaces
{
    public interface ILancamentoAppService : IAppServiceBase<Lancamento>
    {
        Task<Result<string>> PerformOperationAsync(LancamentoRequest lancamentoRequest, CancellationToken cancellationToken);
    }
}