using Superdigital.Domain.Entities;

namespace Superdigital.Domain.ViewModels
{
    public class ContaResponse : BaseResponse
    {
        public ClienteRequest Cliente { get; set; }
        public string Numero { get; set; }
        public string EContaTipoDescricao { get; set; }
        public EContaTipo EContaTipo { get; set; }
        public string SaldoFormatado { get; set; }
        public decimal Saldo { get; set; }
    }
}
