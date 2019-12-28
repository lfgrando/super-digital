using System;
using System.Net;
using System.Threading.Tasks;
using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Repositories;
using Superdigital.Domain.Interfaces.Services;
using Superdigital.Domain.ViewModels;

namespace Superdigital.Domain.Services
{
    public class ContaService : ServiceBase<Conta>, IContaService
    {
        private readonly IContaRepository _contaRepository;
        public ContaService(IContaRepository contaRepository) : base(contaRepository)
        {
            _contaRepository = contaRepository;
        }

        public async Task<Result<bool>> UpdateBalanceAsync(string id, decimal valor)
        {
            var updateBalanceResult = await _contaRepository.UpdateBalanceAsync(id, valor);
            if (!updateBalanceResult)
                return new Result<bool>(false, System.Net.HttpStatusCode.InternalServerError, Failure.GenerateOneFailure("Cannot update balance."));

            return new Result<bool>(true, System.Net.HttpStatusCode.OK, null);
        }
        public Task<Result<string>> GenerateAccountNumberAsync()
        {
            try
            {
                string accountNumber = string.Empty;
                for (int i = 0; i < 8; i++)
                {
                    accountNumber += new Random().Next(0, 9).ToString();
                }
                accountNumber += "-";
                accountNumber += new Random().Next(0, 9).ToString();
                return Task.FromResult(new Result<string>(accountNumber, HttpStatusCode.OK, null));
            }
            catch (Exception ex)
            {
                return Task.FromResult(new Result<string>(null, HttpStatusCode.OK, Failure.GenerateOneFailure(ex.Message)));
            }
        }

        public async Task<Result<bool>> UpdateAccountAsync(ContaRequest conta)
        {
            if (conta == null)
                return new Result<bool>(false, HttpStatusCode.BadRequest, Failure.GenerateOneFailure("Object is null."));

            var objConta = _mapper.Map<ContaRequest, Conta>(conta);
            if (!objConta.IsValid(EValidationStage.Replace))
                return new Result<bool>(false, HttpStatusCode.BadRequest, objConta.ValidationErrors);

            var result = await _contaRepository.UpdateAccountAsync(objConta);
            if (!result)
                return new Result<bool>(false, HttpStatusCode.NotFound, Failure.GenerateOneFailure("Cannot update account."));

            return new Result<bool>(true, HttpStatusCode.OK, null);
        }
    }
}