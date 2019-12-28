using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Superdigital.Domain.Entities;
using Superdigital.Domain.Interfaces.Services;
using Superdigital.Domain.ViewModels.Token;
using System.Net;
using System.Threading.Tasks;

namespace Superdigital.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class LoginController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public LoginController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Post([FromBody]UserRequest userRequest)
        {
            if (userRequest.User != "admin" || userRequest.Password != "admin")
                return StatusCode((int)HttpStatusCode.BadRequest, new Result<string>(null, HttpStatusCode.BadRequest, Failure.GenerateOneFailure("User or password invalid.")));

            string token = await _tokenService.GenerateToken(userRequest.User);

            var userResponse = new UserResponse
            {
                User = userRequest.User,
                Token = token
            };

            return new Result<UserResponse>(userResponse, HttpStatusCode.OK, null);
        }
    }
}