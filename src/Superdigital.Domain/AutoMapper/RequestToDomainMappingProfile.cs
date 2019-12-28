using AutoMapper;
using Superdigital.Domain.Entities;
using Superdigital.Domain.ViewModels;
using Superdigital.Domain.ViewModels.Lancamento;
using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.AutoMapper
{
    public class RequestToResponseMappingProfile : Profile
    {
        public RequestToResponseMappingProfile()
        {
            CreateMap<ClienteRequest, Cliente>();
            CreateMap<ContaRequest, Conta>();
            CreateMap<ContaResponse, Conta>();
            CreateMap<ContaResponse, ContaRequest>();
            CreateMap<LancamentoRequest, Lancamento>();
        }
    }
}
