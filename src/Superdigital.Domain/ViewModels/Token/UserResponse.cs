using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.ViewModels.Token
{
    public class UserResponse : BaseResponse
    {
        public string User { get; set; }
        public string Token { get; set; }
    }
}
