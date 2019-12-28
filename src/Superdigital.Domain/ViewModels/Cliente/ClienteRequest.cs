using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.ViewModels
{
    public class ClienteRequest : BaseRequest
    {
        public string Nome { get; set; }
        public string Email { get; set; }
    }
}
