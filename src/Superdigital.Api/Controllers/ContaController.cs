using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Superdigital.Application.Interfaces;
using Superdigital.Domain.ViewModels;
using Superdigital.Infra.CrossCutting.Extension;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Superdigital.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ContaController : ControllerBase
    {
        HttpRequestMessage request = new HttpRequestMessage();
        private readonly IContaAppService _contaAppService;
        public ContaController(IContaAppService contaAppService)
        {
            _contaAppService = contaAppService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _contaAppService.GetByFilterAsync<ContaResponse>(x => true);

            return
                StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(string id)
        {
            var result = await _contaAppService.GetByIdAsync<ContaResponse>(id);
            return
                StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("Cliente/Name/{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var result = await _contaAppService.GetByFilterAsync<ContaResponse>(x => x.Cliente.Nome.Contains(name));
            return
                StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ContaRequest contaRequest, CancellationToken cancellationToken)
        {
            var validateResult = await _contaAppService.ValidateDuplicateAccountNumberAsync(contaRequest);
            if (!validateResult.StatusCode.IsSuccessStatusCode())
                return StatusCode((int)validateResult.StatusCode, validateResult);

            var result = await _contaAppService.InsertOneAsync<ContaRequest>(contaRequest, cancellationToken);
            return
                StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ContaRequest contaRequest, CancellationToken cancellationToken)
        {
            var validateResult = await _contaAppService.ValidateDuplicateAccountNumberAsync(contaRequest);
            if (!validateResult.StatusCode.IsSuccessStatusCode())
                return StatusCode((int)validateResult.StatusCode, validateResult);

            contaRequest.Id = id;
            var result = await _contaAppService.ReplaceOneAsync<ContaRequest>(x => x.Id == id, contaRequest, null);
            return
                StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            var result = await _contaAppService.DeleteOneAsync(id, cancellationToken);
            return
                StatusCode((int)result.StatusCode, result);
        }

        [HttpGet("Number/Generate")]
        public async Task<ActionResult> GenerateAccountNumber()
        {
            var result = await _contaAppService.GenerateAccountNumberAsync();
            return
                StatusCode((int)result.StatusCode, result);
        }
    }
}