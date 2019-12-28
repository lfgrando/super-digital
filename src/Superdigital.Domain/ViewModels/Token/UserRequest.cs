using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.ViewModels.Token
{
    public class UserRequest : BaseRequest
    {
        public string User { get; set; }
        public string Password { get; set; }
    }
}
