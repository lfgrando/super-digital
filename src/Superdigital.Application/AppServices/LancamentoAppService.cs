using AutoMapper;
using Superdigital.Application.Interfaces;
using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Services;
using Superdigital.Domain.ViewModels;
using Superdigital.Domain.ViewModels.Lancamento;
using Superdigital.Infra.CrossCutting.Extension;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Superdigital.Application.AppServices
{
    public class LancamentoAppService : AppServiceBase<Lancamento>, ILancamentoAppService
    {
        private readonly ILancamentoService _lancamentoService;
        public LancamentoAppService(ILancamentoService lancamentoService) : base(lancamentoService)
        {
            _lancamentoService = lancamentoService;
        }

        public async Task<Result<string>> PerformOperationAsync(LancamentoRequest lancamentoRequest, CancellationToken cancellationToken)
        {
            if (lancamentoRequest == null)
                return new Result<string>(null, HttpStatusCode.BadRequest, Failure.GenerateOneFailure("Object is null."));

            Result<decimal> result;
            switch (lancamentoRequest.EOperacao)
            {
                case EOperacao.Deposito:
                    {
                        result = await _lancamentoService.DepositAsync(lancamentoRequest);
                        break;
                    }
                case EOperacao.Saque:
                    {
                        result = await _lancamentoService.WithdrawAsync(lancamentoRequest);
                        break;
                    }
                case EOperacao.Transferencia:
                    {
                        result = await _lancamentoService.TransferAsync(lancamentoRequest);
                        break;
                    }
                default:
                    return new Result<string>(null, HttpStatusCode.BadRequest, Failure.GenerateOneFailure("Operation kind not supported."));
            }

            if (!result.StatusCode.IsSuccessStatusCode())
                return new Result<string>(null, result.StatusCode, result.Failures);

            return
                await _lancamentoService.InsertOneAsync(lancamentoRequest, cancellationToken);
        }
    }
}
