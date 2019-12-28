using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Repositories;

namespace Superdigital.Infra.Data.Repositories
{
    public class LancamentoRepository : RepositoryBase<Lancamento>, ILancamentoRepository
    {
        public LancamentoRepository(IMongoContext context) : base(context)
        {
        }
    }
}