using MongoDB.Driver;
using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Repositories;
using System.Threading.Tasks;

namespace Superdigital.Infra.Data.Repositories
{
    public class ContaRepository : RepositoryBase<Conta>, IContaRepository
    {
        public ContaRepository(IMongoContext context) : base(context)
        {
        }
        public async Task<bool> UpdateBalanceAsync(string id, decimal valor)
        {
            var updateDefinition = Builders<Conta>.Update.Set(x => x.Saldo, valor);
            var update = await this.Collection.UpdateOneAsync(x => x.Id == id, updateDefinition);
            return
                update.ModifiedCount == 1;
        }

        public async Task<bool> UpdateAccountAsync(Conta conta)
        {
            var updateDefinition = Builders<Conta>.Update.Set(x => x.Cliente.Nome, conta.Cliente.Nome);
            updateDefinition = Builders<Conta>.Update.Set(x => x.Cliente.Email, conta.Cliente.Email);
            updateDefinition = Builders<Conta>.Update.Set(x => x.EContaTipo, conta.EContaTipo);
            updateDefinition = Builders<Conta>.Update.Set(x => x.Numero, conta.Numero);

            var update = await this.Collection.UpdateOneAsync(x => x.Id == conta.Id, updateDefinition);
            return
                update.ModifiedCount == 1;
        }        
    }
}