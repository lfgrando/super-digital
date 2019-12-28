using Superdigital.Domain.Entities;
using System.Threading.Tasks;

namespace Superdigital.Domain.Interfaces.Repositories
{
    public interface IContaRepository : IRepositoryBase<Conta>
    {
        Task<bool> UpdateAccountAsync(Conta conta);
        Task<bool> UpdateBalanceAsync(string id, decimal valor);
    }
}