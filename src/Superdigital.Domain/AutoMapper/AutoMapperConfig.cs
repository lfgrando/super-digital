using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Superdigital.Domain.AutoMapper
{
    public class AutoMapperConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(x =>
            {
                x.AddProfile<ResponseToRequestMappingProfile>();
                x.AddProfile<RequestToResponseMappingProfile>();
            });
        }
    }
}
