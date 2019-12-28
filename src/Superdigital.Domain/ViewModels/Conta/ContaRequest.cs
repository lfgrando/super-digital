using Superdigital.Domain.Entities;

namespace Superdigital.Domain.ViewModels
{
    public class ContaRequest : BaseRequest
    {
        public ClienteRequest Cliente { get; set; }
        public string Numero { get; set; }
        public EContaTipo EContaTipo { get; set; }
    }
}