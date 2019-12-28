using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.ViewModels
{
    public class ClienteResponse : BaseResponse
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
