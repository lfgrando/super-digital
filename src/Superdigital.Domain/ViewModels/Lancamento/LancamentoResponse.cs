using Superdigital.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.ViewModels.Lancamento
{
    public class LancamentoResponse : BaseResponse
    {
        public string ValorFormatado { get; set; }
        public string IdContaOrigem { get; set; }
        public ContaResponse ContaOrigem { get; set; }
        public string IdContaDestino { get; set; }
        public ContaResponse ContaDestino { get; set; }
        public string EOperacaoDescricao { get; set; }
        public string DataCriacaoFormatada { get; set; }
    }
}