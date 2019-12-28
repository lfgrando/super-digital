using Superdigital.Domain.ViewModels.Token;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Superdigital.Domain.Interfaces.Services
{
    public interface ITokenService
    {
        Task<string> GenerateToken(string user);
    }
}
