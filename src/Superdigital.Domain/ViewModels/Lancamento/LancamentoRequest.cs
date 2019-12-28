using Superdigital.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.ViewModels.Lancamento
{
    public class LancamentoRequest : BaseRequest
    {
        public decimal Valor { get; set; }
        public string IdContaOrigem { get; set; }
        public ContaRequest ContaOrigem { get; set; }
        public string IdContaDestino { get; set; }
        public ContaRequest ContaDestino { get; set; }
        public EOperacao EOperacao { get; set; }
    }
}
