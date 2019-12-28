using Superdigital.Application.Interfaces;
using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Services;
using Superdigital.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Superdigital.Application.AppServices
{
    public class ContaAppService : AppServiceBase<Conta>, IContaAppService
    {
        private readonly IContaService _contaService;
        public ContaAppService(IContaService pContaService) : base(pContaService)
        {
            _contaService = pContaService;
        }

        public async Task<Result<bool>> ValidateDuplicateAccountNumberAsync(ContaRequest contaRequest)
        {
            if(contaRequest == null)
                return new Result<bool>(false, HttpStatusCode.BadRequest, Failure.GenerateOneFailure("Object is null."));

            var account = await _contaService.GetByFilterAsync<ContaResponse>(x => x.Numero == contaRequest.Numero && x.Id != contaRequest.Id);

            if (account.Value?.Count > 0)
                return new Result<bool>(false, HttpStatusCode.BadRequest, Failure.GenerateOneFailure("The account number is already in use."));

            return new Result<bool>(true, HttpStatusCode.OK, null);
        }
        public async Task<Result<string>> GenerateAccountNumberAsync()
        {
            return
                await _contaService.GenerateAccountNumberAsync();
        }
        public async Task<Result<bool>> UpdateAccountAsync(ContaRequest conta)
        {
            return
                await _contaService.UpdateAccountAsync(conta);
        }
    }
}