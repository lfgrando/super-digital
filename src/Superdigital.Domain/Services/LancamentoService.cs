using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Repositories;
using Superdigital.Domain.Interfaces.Services;
using Superdigital.Domain.ViewModels;
using Superdigital.Domain.ViewModels.Lancamento;
using Superdigital.Infra.CrossCutting.Extension;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Superdigital.Domain.Services
{
    public class LancamentoService : ServiceBase<Lancamento>, ILancamentoService
    {
        private readonly IContaService _contaService;
        public LancamentoService(ILancamentoRepository pLancamentoService, IContaService contaService) : base(pLancamentoService)
        {
            _contaService = contaService;
        }
        public async Task<Result<decimal>> DepositAsync(LancamentoRequest lancamentoRequest)
        {
            try
            {
                var validation = await ValidateOperation(lancamentoRequest);
                if (!validation.StatusCode.IsSuccessStatusCode())
                    return validation;

                var targetAccount = await _contaService.GetByIdAsync<ContaResponse>(lancamentoRequest.IdContaDestino);
                var targetAccountRequest = _mapper.Map<ContaResponse, ContaRequest>(targetAccount.Value);
                lancamentoRequest.ContaDestino = targetAccountRequest;

                if (!targetAccount.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, targetAccount.StatusCode, targetAccount.Failures);

                var account = base._mapper.Map<ContaResponse, Conta>(targetAccount.Value);
                var newBalance = await account.IncreaseBalance(lancamentoRequest.Valor);
                if (!newBalance.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, newBalance.StatusCode, newBalance.Failures);

                var updateBalance = await _contaService.UpdateBalanceAsync(account.Id, newBalance.Value);
                if (!updateBalance.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, updateBalance.StatusCode, updateBalance.Failures);

                return new Result<decimal>(newBalance.Value, HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                return new Result<decimal>(0, HttpStatusCode.InternalServerError, Failure.GenerateOneFailure(ex.Message));
            }
        }

        public async Task<Result<decimal>> WithdrawAsync(LancamentoRequest lancamentoRequest)
        {
            try
            {
                var validation = await ValidateOperation(lancamentoRequest);
                if (!validation.StatusCode.IsSuccessStatusCode())
                    return validation;

                var sourceAccount = await _contaService.GetByIdAsync<ContaResponse>(lancamentoRequest.IdContaOrigem);
                var sourceAccountRequest = _mapper.Map<ContaResponse, ContaRequest>(sourceAccount.Value);
                lancamentoRequest.ContaOrigem = sourceAccountRequest;

                if (!sourceAccount.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, sourceAccount.StatusCode, sourceAccount.Failures);

                var account = base._mapper.Map<ContaResponse, Conta>(sourceAccount.Value);
                var newBalance = await account.DecreaseBalance(lancamentoRequest.Valor);
                if (!newBalance.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, newBalance.StatusCode, newBalance.Failures);

                var updateBalance = await _contaService.UpdateBalanceAsync(account.Id, newBalance.Value);
                if (!updateBalance.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, updateBalance.StatusCode, updateBalance.Failures);

                return new Result<decimal>(newBalance.Value, HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                return new Result<decimal>(0, HttpStatusCode.InternalServerError, Failure.GenerateOneFailure(ex.Message));
            }
        }

        public async Task<Result<decimal>> TransferAsync(LancamentoRequest lancamentoRequest)
        {
            try
            {
                #region Validate the operation
                var validation = await ValidateOperation(lancamentoRequest);
                if (!validation.StatusCode.IsSuccessStatusCode())
                    return validation;
                #endregion
                #region Map the account object, so we can track the operation
                var sourceAccountResponse = await _contaService.GetByIdAsync<ContaResponse>(lancamentoRequest.IdContaOrigem);
                var targetAccountResponse = await _contaService.GetByIdAsync<ContaResponse>(lancamentoRequest.IdContaDestino);

                var sourceAccountRequest = _mapper.Map<ContaResponse, ContaRequest>(sourceAccountResponse.Value);
                var targetAccountRequest = _mapper.Map<ContaResponse, ContaRequest>(targetAccountResponse.Value);

                lancamentoRequest.ContaOrigem = sourceAccountRequest;
                lancamentoRequest.ContaDestino = targetAccountRequest;

                if (!sourceAccountResponse.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, sourceAccountResponse.StatusCode, sourceAccountResponse.Failures);

                if (!targetAccountResponse.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, targetAccountResponse.StatusCode, targetAccountResponse.Failures);

                var sourceAccount = base._mapper.Map<ContaResponse, Conta>(sourceAccountResponse.Value);
                var targetAccount = base._mapper.Map<ContaResponse, Conta>(targetAccountResponse.Value);
                #endregion
                #region Decrease Balance
                var newBalanceSourceAccount = await sourceAccount.DecreaseBalance(lancamentoRequest.Valor);
                if (!newBalanceSourceAccount.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, newBalanceSourceAccount.StatusCode, newBalanceSourceAccount.Failures);
                #endregion
                #region Increase Balance
                var newBalanceTargetAccount = await targetAccount.IncreaseBalance(lancamentoRequest.Valor);
                if (!newBalanceTargetAccount.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, newBalanceTargetAccount.StatusCode, newBalanceTargetAccount.Failures);
                #endregion
                #region Update Balance
                var updateBalance = await _contaService.UpdateBalanceAsync(sourceAccount.Id, newBalanceSourceAccount.Value);
                if (!updateBalance.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, updateBalance.StatusCode, updateBalance.Failures);

                updateBalance = await _contaService.UpdateBalanceAsync(targetAccount.Id, newBalanceTargetAccount.Value);
                if (!updateBalance.StatusCode.IsSuccessStatusCode())
                    return new Result<decimal>(0, updateBalance.StatusCode, updateBalance.Failures);
                #endregion

                return new Result<decimal>(newBalanceSourceAccount.Value, HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                return new Result<decimal>(0, HttpStatusCode.InternalServerError, Failure.GenerateOneFailure(ex.Message));
            }
        }

        private Task<Result<decimal>> ValidateOperation(LancamentoRequest lancamentoRequest)
        {
            var objLancamento = base._mapper.Map<LancamentoRequest, Lancamento>(lancamentoRequest);
            if (!objLancamento.IsValid(EValidationStage.Insert))
                return Task.FromResult(new Result<decimal>(0, HttpStatusCode.BadRequest, objLancamento.ValidationErrors));

            return Task.FromResult(new Result<decimal>(0, HttpStatusCode.OK, null));
        }

    }
}
