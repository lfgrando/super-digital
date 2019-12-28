using MongoDB.Driver;
using NSubstitute;
using Superdigital.Application.AppServices;
using Superdigital.Application.Interfaces;
using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Repositories;
using Superdigital.Domain.Interfaces.Services;
using Superdigital.Domain.Services;
using Superdigital.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Superdigital.Tests.UnitTests
{
    public class ContaTest
    {
        private readonly IContaAppService _contaApp;
        private readonly IContaService _contaService;
        private readonly IContaRepository _contaRepository;
        private readonly CancellationToken _cancellationToken;

        public ContaTest()
        {
            _contaRepository = Substitute.For<IContaRepository>();
            _contaService = new ContaService(_contaRepository);
            _contaApp = new ContaAppService(_contaService);
            _cancellationToken = new CancellationToken();
            SetupRepository();
        }

        [Fact]
        public async void TestCreateContaOk()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "0102030-10",
                Cliente = new ClienteRequest
                {
                    Id = "ABC",
                    Nome = "Renan",
                    Email = "renan.ranciaro@gmail.com"
                }
            };
            var result = await _contaApp.InsertOneAsync(objConta, _cancellationToken);
            Assert.True(result.Failures == null);
            Assert.True(result.StatusCode == HttpStatusCode.Created);
            Assert.True(result.Value == "1");
        }

        [Fact]
        public async void TestCreateContaObjectNullError()
        {
            var result = await _contaApp.InsertOneAsync<ContaRequest>(null, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Object is null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateContaNumeroEmptyError()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "",
                Cliente = new ClienteRequest
                {
                    Id = "ABC",
                    Nome = "Renan",
                    Email = "renan.ranciaro@gmail.com"
                }
            };
            var result = await _contaApp.InsertOneAsync(objConta, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Numero can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateContaClienteNullError()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "0102030-10",
                Cliente = null
            };
            var result = await _contaApp.InsertOneAsync(objConta, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Cliente can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateContaClienteNomeEmptyError()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "0102030-10",
                Cliente = new ClienteRequest
                {
                    Id = "ABC",
                    Nome = "",
                    Email = "renan.ranciaro@gmail.com"
                }
            };
            var result = await _contaApp.InsertOneAsync(objConta, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Cliente.Nome can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateContaClienteEmailEmptyError()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "0102030-10",
                Cliente = new ClienteRequest
                {
                    Id = "ABC",
                    Nome = "Renan",
                    Email = ""
                }
            };
            var result = await _contaApp.InsertOneAsync(objConta, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Cliente.Email can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestCreateContaClienteEContaTipoInvalidError()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "0102030-10",
                EContaTipo = (EContaTipo)999,
                Cliente = new ClienteRequest
                {
                    Id = "ABC",
                    Nome = "Renan",
                    Email = "renan.ranciaro@gmail.com"
                }
            };
            var result = await _contaApp.InsertOneAsync(objConta, _cancellationToken);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "EContaTipo is invalid.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestGetContaNotFound()
        {
            var result = await _contaApp.GetByIdAsync<ContaResponse>("0");

            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Conta not found.");
            Assert.True(result.Value == null);
        }

        [Fact]
        public async void TestGetContaNullError()
        {
            var result = await _contaApp.GetByIdAsync<ContaResponse>(null);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Conta not found.");
            Assert.True(result.Value == null);
        }

        [Fact]
        public async void TestGetContaOk()
        {
            var result = await _contaApp.GetByIdAsync<ContaResponse>("1");
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(result.Failures == null);
            Assert.True(result.Value.Id == "ABC");
        }

        [Fact]
        public async void TestReplaceContaErrorNotFound()
        {
            var objConta = new ContaRequest
            {
                Id = "0",
                Numero = "0102030-10",
                Cliente = new ClienteRequest
                {
                    Id = "ABC",
                    Nome = "Renan",
                    Email = "renan.ranciaro@gmail.com"
                }
            };
            var result = await _contaApp.ReplaceOneAsync(x => x.Id == "0", objConta, null);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Conta not found.");
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceContaObjectNullError()
        {
            var result = await _contaApp.ReplaceOneAsync<ContaRequest>(x => x.Id == "ABC", null, null);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Object is null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceContaNumeroEmptyError()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "",
                Cliente = new ClienteRequest
                {
                    Id = "ABC",
                    Nome = "Renan",
                    Email = "renan.ranciaro@gmail.com"
                }
            };
            var result = await _contaApp.ReplaceOneAsync(x => x.Id == "ABC", objConta, null);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Numero can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceContaClienteNullError()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "0102030-10",
                Cliente = null
            };
            var result = await _contaApp.ReplaceOneAsync(x => x.Id == "ABC", objConta, null);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Cliente can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceContaClienteNomeEmptyError()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "0102030-10",
                Cliente = new ClienteRequest
                {
                    Id = "ABC",
                    Nome = "",
                    Email = "renan.ranciaro@gmail.com"
                }
            };
            var result = await _contaApp.ReplaceOneAsync(x => x.Id == "ABC", objConta, null);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Cliente.Nome can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceContaClienteEmailEmptyError()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "0102030-10",
                Cliente = new ClienteRequest
                {
                    Id = "ABC",
                    Nome = "Renan",
                    Email = ""
                }
            };
            var result = await _contaApp.ReplaceOneAsync(x => x.Id == "ABC", objConta, null);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Cliente.Email can't be null.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceContaPredicateNullError()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC"
            };

            var result = await _contaApp.ReplaceOneAsync(null, objConta, null);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Filter definition can't be null.");
            Assert.True(result.Value == null);
        }

        [Fact]
        public async void TestReplaceContaClienteEContaTipoInvalidError()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "0102030-10",
                EContaTipo = (EContaTipo)999,
                Cliente = new ClienteRequest
                {
                    Id = "ABC",
                    Nome = "Renan",
                    Email = "renan.ranciaro@gmail.com"
                }
            };
            var result = await _contaApp.ReplaceOneAsync(x => x.Id == "ABC", objConta, null);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "EContaTipo is invalid.");
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(string.IsNullOrWhiteSpace(result.Value));
        }

        [Fact]
        public async void TestReplaceContaOk()
        {
            var objConta = new ContaRequest()
            {
                Id = "ABC",
                Numero = "0102030-10",
                Cliente = new ClienteRequest
                {
                    Id = "ABC",
                    Nome = "Renan",
                    Email = "renan.ranciaro@gmail.com"
                }
            };
            var result = await _contaApp.ReplaceOneAsync(x => x.Id == "ABC", objConta, null);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(result.Failures == null);
            Assert.True(result.Value == "1");
        }

        [Fact]
        public async void TestDeleteContaNotFound()
        {
            var result = await _contaApp.DeleteOneAsync("0", _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Conta not found.");
            Assert.True(result.Value == 0);
        }

        [Fact]
        public async void TestDeleteContaIdNullError()
        {
            var result = await _contaApp.DeleteOneAsync(null, _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.True(result.Value == 0);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Id can't be null.");
        }

        [Fact]
        public async void TestDeleteContaOk()
        {
            var result = await _contaApp.DeleteOneAsync("ABC", _cancellationToken);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(result.Failures == null);
            Assert.True(result.Value == 1);
        }

        [Fact]
        public async void TestGenerateAccountNumberOk()
        {
            var result = await _contaApp.GenerateAccountNumberAsync();
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(result.Failures == null);
            Assert.True(result.Value.Length == 10);
        }

        [Fact]
        public async void TestValidateDuplicateAccountRequestNullError()
        {
            _contaRepository.GetByFilterAsync(null).ReturnsForAnyArgs(new List<Conta> { new Conta { Id = "ABC", Cliente = new Cliente { Nome = "Renan" } } });
            var result = await _contaApp.ValidateDuplicateAccountNumberAsync(null);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Object is null.");
            Assert.True(!result.Value);
        }

        [Fact]
        public async void TestValidateDuplicateAccountDuplicateError()
        {
            var contaRequest = new ContaRequest
            {
                Cliente = new ClienteRequest
                {
                    Email = "renan.ranciaro@gmail.com",
                    Nome = "Renan",
                    Id = "ABC"
                },
                EContaTipo = EContaTipo.Corrente,
                Id = "0",
                Numero = "0102030-1"
            };
            _contaRepository.GetByFilterAsync(null).ReturnsForAnyArgs(new List<Conta> { new Conta { Id = "ABC", Cliente = new Cliente { Nome = "Renan" } } });
            var result = await _contaApp.ValidateDuplicateAccountNumberAsync(contaRequest);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "The account number is already in use.");
            Assert.True(!result.Value);
        }

        [Fact]
        public async void TestValidateDuplicateAccountNumberOk()
        {
            var contaRequest = new ContaRequest
            {
                Cliente = new ClienteRequest
                {
                    Email = "renan.ranciaro@gmail.com",
                    Nome = "Renan",
                    Id = "ABC"
                },
                EContaTipo = EContaTipo.Corrente,
                Id = "0",
                Numero = "0102030-1"
            };
            _contaRepository.GetByFilterAsync(null).ReturnsForAnyArgs((ICollection<Conta>)null);
            var result = await _contaApp.ValidateDuplicateAccountNumberAsync(contaRequest);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(result.Failures == null);
            Assert.True(result.Value);
        }

        [Fact]
        public async void TestUpdateAccountNullError()
        {
            var result = await _contaApp.UpdateAccountAsync(null);
            Assert.True(result.StatusCode == HttpStatusCode.BadRequest);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Object is null.");
            Assert.True(!result.Value);
        }

        [Fact]
        public async void TestUpdateAccountNotFoundError()
        {
            var conta = new ContaRequest
            {
                Id = "0",
                Numero = "0102030-10",
                EContaTipo = EContaTipo.Corrente,
                Cliente = new ClienteRequest
                {
                    Email = "renan.ranciaro@gmail.com",
                    Nome = "Renan Ranciaro"
                }
            };
            var result = await _contaApp.UpdateAccountAsync(conta);
            Assert.True(result.StatusCode == HttpStatusCode.NotFound);
            Assert.Contains(result.Failures, x => x.ErrorMessage == "Cannot update account.");
            Assert.True(!result.Value);
        }

        [Fact]
        public async void TestUpdateAccountOk()
        {
            var conta = new ContaRequest
            {
                Id = "ABC",
                Numero = "0102030-10",
                EContaTipo = EContaTipo.Corrente,
                Cliente = new ClienteRequest
                {
                    Email = "renan.ranciaro@gmail.com",
                    Nome = "Renan Ranciaro"
                }
            };
            var result = await _contaApp.UpdateAccountAsync(conta);
            Assert.True(result.StatusCode == HttpStatusCode.OK);
            Assert.True(result.Failures == null);
            Assert.True(result.Value);
        }

        private void SetupRepository()
        {
            #region Inserts
            _contaRepository.InsertOneAsync(null, Arg.Any<CancellationToken>()).Returns(Task.FromResult(string.Empty));
            _contaRepository.InsertOneAsync(Arg.Is<Conta>(x => x.Id == "0"), Arg.Any<CancellationToken>()).Returns(Task.FromResult(string.Empty));
            _contaRepository.InsertOneAsync(Arg.Is<Conta>(x => x.Id == "ABC"), Arg.Any<CancellationToken>()).Returns(Task.FromResult("1"));
            #endregion
            #region Gets
            _contaRepository.GetByIdAsync("0").Returns(Task.FromResult(default(Conta)));
            _contaRepository.GetByIdAsync("1").Returns(Task.FromResult(new Conta()
            {
                Id = "ABC",
                Numero = "010552-2",
                Cliente = new Cliente
                {
                    Nome = "Renan",
                    Id = "ABC"
                }
            }));
            #endregion
            #region Delete
            _contaRepository.DeleteOneAsync("ABC", Arg.Any<CancellationToken>()).Returns(Task.FromResult(long.Parse("1")));
            _contaRepository.DeleteOneAsync("0", Arg.Any<CancellationToken>()).Returns(Task.FromResult(long.Parse("0")));
            #endregion
            #region Update
            _contaRepository.ReplaceOneAsync(Arg.Any<Expression<Func<Conta, bool>>>(), Arg.Is<Conta>(x => x.Id == "0"), Arg.Any<UpdateOptions>()).Returns(Task.FromResult(""));
            _contaRepository.ReplaceOneAsync(Arg.Any<Expression<Func<Conta, bool>>>(), Arg.Is<Conta>(x => x.Id == "ABC"), Arg.Any<UpdateOptions>()).Returns(Task.FromResult("1"));
            _contaRepository.UpdateAccountAsync(Arg.Is<Conta>(x => x.Id == "0")).Returns(false);
            _contaRepository.UpdateAccountAsync(Arg.Is<Conta>(x => x.Id == "ABC")).Returns(true);
            #endregion
        }
    }
}