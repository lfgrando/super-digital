using MongoDB.Driver;
using NSubstitute;
using Superdigital.Application.AppServices;
using Superdigital.Application.Interfaces;
using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Repositories;
using Superdigital.Domain.Interfaces.Services;
using Superdigital.Domain.Services;
using Superdigital.Domain.ViewModels.Lancamento;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Superdigital.Tests.UnitTests
{
    public class LancamentoTest
    {
        private readonly ILancamentoAppService _lancamentoApp;
        private readonly IContaService _contaService;
        private readonly IContaRepository _contaRepository;
        private readonly ILancamentoService _lancamentoService;
        private readonly ILancamentoRepository _lancamentoRepository;
        private readonly CancellationToken _cancellationToken;

        public LancamentoTest()
        {
            _lancamentoRepository = Substitute.For<ILancamentoRepository>();
            _contaRepository = Substitute.For<IContaRepository>();
            _contaService = new ContaService(_contaRepository);            
            _lancamentoService = new LancamentoService(_lancamentoRepository, _contaService);
            _lancamentoApp = new LancamentoAppService(_lancamentoService);
            _cancellationToken = new CancellationToken();
            SetupRepository();
        }

        [Fact]
        public async void TestCreateLancamentoOk()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "AAA",
                IdContaOrigem = "BBB",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.True(result.Failures == null);
            Assert.True(result.StatusCode == HttpStatusCode.Created);
            Assert.True(result.Value == "ABC");
        }

        [Fact]
        public async void TestCreateLancamentoObjectNullError()
        {
            var result = await _lancamentoApp.InsertOneAsync<LancamentoRequest>(null, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Object is null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateLancamentoEOperacaoInvalidError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = (EOperacao)999,
                IdContaDestino = "01020-30",
                IdContaOrigem = "01030-20",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "EOperacao is invalid.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateLancamentoDepositoContaDestinoEmptyError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Deposito,
                IdContaDestino = "",
                IdContaOrigem = "01030-20",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaDestino can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateLancamentoDepositoContaOrigemEmptyOk()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Deposito,
                IdContaDestino = "01030-30",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.True(result.Failures == null);
            Assert.True(result.StatusCode == HttpStatusCode.Created);
            Assert.True(result.Value == "ABC");
        }

        [Fact]
        public async void TestCreateLancamentoSaqueContaOrigemEmptyError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Saque,
                IdContaDestino = "01020-30",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaOrigem can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateLancamentoSaqueContaDestinoEmptyOk()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Saque,
                IdContaDestino = "",
                IdContaOrigem = "01020-30",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.True(result.Failures == null);
            Assert.True(result.StatusCode == HttpStatusCode.Created);
            Assert.True(result.Value == "ABC");
        }

        [Fact]
        public async void TestCreateLancamentoTransferenciaContaOrigemEmptyError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "01020-30",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaOrigem can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateLancamentoTransferenciaContaDestinoEmptyError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "",
                IdContaOrigem = "01020-30",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaDestino can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateLancamentoTransferenciaContaOrigemSameThatContaDestinoDestinoError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "01020-30",
                IdContaOrigem = "01020-30",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaOrigem can't be the same that IdContaDestino.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateLancamentoValorZeroError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "01020-20",
                IdContaOrigem = "01020-30",
                Valor = 0
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Valor must be highter than R$ 0,00.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceLancamentoOk()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "AAA",
                IdContaOrigem = "BBB",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.True(result.Failures == null);
            Assert.True(result.StatusCode == HttpStatusCode.Created);
            Assert.True(result.Value == "ABC");
        }

        [Fact]
        public async void TestReplaceLancamentoObjectNullError()
        {
            var result = await _lancamentoApp.InsertOneAsync<LancamentoRequest>(null, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Object is null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceLancamentoEOperacaoInvalidError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = (EOperacao)999,
                IdContaDestino = "01020-30",
                IdContaOrigem = "01030-20",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "EOperacao is invalid.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceLancamentoDepositoContaDestinoEmptyError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Deposito,
                IdContaDestino = "",
                IdContaOrigem = "01030-20",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaDestino can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceLancamentoDepositoContaOrigemEmptyOk()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Deposito,
                IdContaDestino = "01030-30",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.True(result.Failures == null);
            Assert.True(result.StatusCode == HttpStatusCode.Created);
            Assert.True(result.Value == "ABC");
        }

        [Fact]
        public async void TestReplaceLancamentoSaqueContaOrigemEmptyError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Saque,
                IdContaDestino = "01020-30",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaOrigem can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceLancamentoSaqueContaDestinoEmptyOk()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Saque,
                IdContaDestino = "",
                IdContaOrigem = "01020-30",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.True(result.Failures == null);
            Assert.True(result.StatusCode == HttpStatusCode.Created);
            Assert.True(result.Value == "ABC");
        }

        [Fact]
        public async void TestReplaceLancamentoTransferenciaContaOrigemEmptyError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "01020-30",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaOrigem can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceLancamentoTransferenciaContaDestinoEmptyError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "",
                IdContaOrigem = "01020-30",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaDestino can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceLancamentoTransferenciaContaOrigemSameThatContaDestinoDestinoError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "01020-30",
                IdContaOrigem = "01020-30",
                Valor = 100
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaOrigem can't be the same that IdContaDestino.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceLancamentoValorZeroError()
        {
            var objLancamento = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "01020-20",
                IdContaOrigem = "01020-30",
                Valor = 0
            };
            var result = await _lancamentoApp.InsertOneAsync(objLancamento, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Valor must be highter than R$ 0,00.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestGetLancamentoNotFound()
        {
            var result = await _lancamentoApp.GetByIdAsync<LancamentoResponse>("0");

            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Lancamento not found.");
            Assert.True(result.Value == null);
        }

        [Fact]
        public async void TestGetLancamentoNullError()
        {
            var result = await _lancamentoApp.GetByIdAsync<LancamentoResponse>(null);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Lancamento not found.");
            Assert.True(result.Value == null);
        }

        [Fact]
        public async void TestGetLancamentoOk()
        {
            var result = await _lancamentoApp.GetByIdAsync<LancamentoResponse>("1");
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(result.Failures == null);
            Assert.True(result.Value.Id == "ABC");
        }

        [Fact]
        public async void TestDeleteLancamentoNotFound()
        {
            var result = await _lancamentoApp.DeleteOneAsync("0", _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Lancamento not found.");
            Assert.True(result.Value == 0);
        }

        [Fact]
        public async void TestDeleteLancamentoIdNullError()
        {
            var result = await _lancamentoApp.DeleteOneAsync(null, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(result.Value == 0);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Id can't be null.");
        }

        [Fact]
        public async void TestDeleteLancamentoOk()
        {
            var result = await _lancamentoApp.DeleteOneAsync("ABC", _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(result.Failures == null);
            Assert.True(result.Value == 1);
        }

        [Fact]
        public async void TestPerformOperationInvalidOperationError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = (EOperacao)999,
                IdContaDestino = "",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Operation kind not supported.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestPerformOperationLancamentoRequestNullError()
        {
            var result = await _lancamentoApp.PerformOperationAsync(null, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Object is null.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestDepositIdContaDestinoEmptyError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Deposito,
                IdContaDestino = "",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaDestino can't be null.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestDepositIdContaDestinoNotFoundError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Deposito,
                IdContaDestino = "0",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Conta not found.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestDepositUpdateBalanceError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Deposito,
                IdContaDestino = "1",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.InternalServerError);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Cannot update balance.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestDepositOk()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Deposito,
                IdContaDestino = "ABC",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.Created);
            Assert.True(result.Failures == null);
            Assert.True(result.Value == "ABC");
        }

        [Fact]
        public async void TestWithdrawIdContaOrigemEmptyError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Saque,
                IdContaDestino = "",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaOrigem can't be null.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestWithdrawIdContaOrigemNotFoundError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Saque,
                IdContaDestino = "",
                IdContaOrigem = "0",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Conta not found.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestWithdrawUpdateBalanceError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Deposito,
                IdContaDestino = "1",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.InternalServerError);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Cannot update balance.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestWithdrawInsufficientFundsError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Saque,
                IdContaDestino = "",
                IdContaOrigem = "ABC",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Insufficient funds.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestWithdrawOk()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Saque,
                IdContaDestino = "",
                IdContaOrigem = "2",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.Created);
            Assert.True(result.Failures == null);
            Assert.True(result.Value == "ABC");
        }

        [Fact]
        public async void TestTransferIdContaOrigemEmptyError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaOrigem can't be null.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestTransferIdContaDestinoEmptyError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "",
                IdContaOrigem = "",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaDestino can't be null.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestTransferIdContaOrigemSameThatIdContaDestinoError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "10",
                IdContaOrigem = "10",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "IdContaOrigem can't be the same that IdContaDestino.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestTransferIdContaOrigemNotFoundError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "1",
                IdContaOrigem = "0",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Conta not found.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestTransferIdContaDestinoNotFoundError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "0",
                IdContaOrigem = "1",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Conta not found.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestTransferUpdateBalanceError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "3",
                IdContaOrigem = "1",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.InternalServerError);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Cannot update balance.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestTransferInsufficientFundsError()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "2",
                IdContaOrigem = "3",
                Valor = 200
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Insufficient funds.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestTransferOk()
        {
            var objLancamentoRequest = new LancamentoRequest()
            {
                Id = "ABC",
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "3",
                IdContaOrigem = "2",
                Valor = 100
            };
            var result = await _lancamentoApp.PerformOperationAsync(objLancamentoRequest, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.Created);
            Assert.True(result.Failures == null);
            Assert.True(result.Value == "ABC");
        }

        private void SetupRepository()
        {
            #region Inserts
            _lancamentoRepository.InsertOneAsync(null, Arg.Any<CancellationToken>()).Returns(Task.FromResult(string.Empty));
            _lancamentoRepository.InsertOneAsync(Arg.Is<Lancamento>(x => x.Id == "0"), Arg.Any<CancellationToken>()).Returns(Task.FromResult(string.Empty));
            _lancamentoRepository.InsertOneAsync(Arg.Is<Lancamento>(x => x.Id == "ABC"), Arg.Any<CancellationToken>()).Returns(Task.FromResult("ABC"));
            #endregion
            #region Gets
            _lancamentoRepository.GetByIdAsync("0").Returns(Task.FromResult(default(Lancamento)));
            _lancamentoRepository.GetByIdAsync("1").Returns(Task.FromResult(new Lancamento()
            {
                Id = "ABC",
                DataCriacao = DateTime.UtcNow,
                EOperacao = EOperacao.Transferencia,
                IdContaDestino = "AAA",
                IdContaOrigem = "BBB",
                Valor = 100
            }));
            #endregion
            #region Delete
            _lancamentoRepository.DeleteOneAsync("ABC", Arg.Any<CancellationToken>()).Returns(Task.FromResult(long.Parse("1")));
            _lancamentoRepository.DeleteOneAsync("0", Arg.Any<CancellationToken>()).Returns(Task.FromResult(long.Parse("0")));
            #endregion
            #region Update
            _lancamentoRepository.ReplaceOneAsync(Arg.Any<Expression<Func<Lancamento, bool>>>(), Arg.Is<Lancamento>(x => x.Id == "0"), Arg.Any<UpdateOptions>()).Returns(Task.FromResult(""));
            _lancamentoRepository.ReplaceOneAsync(Arg.Any<Expression<Func<Lancamento, bool>>>(), Arg.Is<Lancamento>(x => x.Id == "ABC"), Arg.Any<UpdateOptions>()).Returns(Task.FromResult("1"));
            #endregion
            #region Conta
            _contaRepository.GetByIdAsync(Arg.Is<string>("ABC")).Returns(new Conta()
            {
                Id = "ABC",
                Cliente = new Cliente
                {
                    Nome = "Renan",
                    Email = "renan.ranciaro@gmail.com",
                    Id = "ABC",
                    DataCriacao = DateTime.UtcNow
                },
                EContaTipo = EContaTipo.Corrente,
                Numero = "01020-30",
                DataCriacao = DateTime.UtcNow,
            });
            _contaRepository.GetByIdAsync(Arg.Is<string>("1")).Returns(new Conta(100)
            {
                Id = "1",
                Cliente = new Cliente
                {
                    Nome = "Renan",
                    Email = "renan.ranciaro@gmail.com",
                    Id = "ABC",
                    DataCriacao = DateTime.UtcNow
                },
                EContaTipo = EContaTipo.Corrente,
                Numero = "01020-30",
                DataCriacao = DateTime.UtcNow,
            });
            _contaRepository.GetByIdAsync(Arg.Is<string>("2")).Returns(new Conta(100)
            {
                Id = "2",
                Cliente = new Cliente
                {
                    Nome = "Renan",
                    Email = "renan.ranciaro@gmail.com",
                    Id = "ABC",
                    DataCriacao = DateTime.UtcNow
                },
                EContaTipo = EContaTipo.Corrente,
                Numero = "01020-30",
                DataCriacao = DateTime.UtcNow,
            });
            _contaRepository.GetByIdAsync(Arg.Is<string>("3")).Returns(new Conta(100)
            {
                Id = "3",
                Cliente = new Cliente
                {
                    Nome = "Renan",
                    Email = "renan.ranciaro@gmail.com",
                    Id = "ABC",
                    DataCriacao = DateTime.UtcNow
                },
                EContaTipo = EContaTipo.Corrente,
                Numero = "01020-30",
                DataCriacao = DateTime.UtcNow,
            });
            _contaRepository.GetByIdAsync("0").Returns(default(Conta));
            _contaRepository.UpdateBalanceAsync("2", Arg.Any<decimal>()).Returns(true);
            _contaRepository.UpdateBalanceAsync("3", Arg.Any<decimal>()).Returns(true);
            _contaRepository.UpdateBalanceAsync("ABC", Arg.Any<decimal>()).Returns(true);
            _contaRepository.UpdateBalanceAsync("1", Arg.Any<decimal>()).Returns(false);
            #endregion
        }
    }
}
