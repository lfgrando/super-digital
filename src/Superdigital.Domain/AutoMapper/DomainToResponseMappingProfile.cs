using AutoMapper;
using Superdigital.Domain.Entities;
using Superdigital.Domain.ViewModels;
using Superdigital.Domain.ViewModels.Lancamento;
using Superdigital.Infra.CrossCutting.Extension;
using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.AutoMapper
{
    public class ResponseToRequestMappingProfile : Profile
    {
        public ResponseToRequestMappingProfile()
        {
            CreateMap<Cliente, ClienteResponse>();
            CreateMap<Cliente, ClienteRequest>();
            CreateMap<Conta, ContaResponse>().ForMember(x => x.SaldoFormatado, y => y.MapFrom(c => c.Saldo.FormatValue()))
                                             .ForMember(x => x.EContaTipoDescricao, y => y.MapFrom(c => c.EContaTipo.GetDescription()));                                             
            CreateMap<Conta, ContaRequest>();
            CreateMap<Lancamento, LancamentoRequest>();
            CreateMap<Lancamento, LancamentoResponse>().ForMember(x => x.EOperacaoDescricao, y => y.MapFrom(c => c.EOperacao.GetDescription()))
                                                       .ForMember(x => x.ValorFormatado, y => y.MapFrom(c => c.Valor.FormatValue()))
                                                       .ForMember(x => x.DataCriacaoFormatada, y => y.MapFrom(c => c.DataCriacao.FormatValue()));
        }
    }
}
