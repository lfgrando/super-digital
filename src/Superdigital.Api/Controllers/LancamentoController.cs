using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Superdigital.Application.Interfaces;
using Superdigital.Domain.ViewModels.Lancamento;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Superdigital.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class LancamentoController : ControllerBase
    {
        HttpRequestMessage request = new HttpRequestMessage();
        private readonly ILancamentoAppService _lancamentoAppService;
        public LancamentoController(ILancamentoAppService lancamentoAppService)
        {
            _lancamentoAppService = lancamentoAppService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(LancamentoRequest lancamentoRequest, CancellationToken cancellationToken)
        {
            var result = await _lancamentoAppService.PerformOperationAsync(lancamentoRequest, cancellationToken);
            return
                StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{idConta}")]
        public async Task<ActionResult> Get(string idConta)
        {
            var result = await _lancamentoAppService.GetByFilterAsync<LancamentoResponse>(x => x.IdContaDestino == idConta || x.IdContaOrigem == idConta);

            return
                StatusCode((int)result.StatusCode, result);
        }
    }
}