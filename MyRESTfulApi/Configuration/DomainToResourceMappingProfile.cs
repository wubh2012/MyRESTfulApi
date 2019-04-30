using AutoMapper;
using MyRESTfulApi.Models;
using MyRESTfulApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRESTfulApi.Configuration
{
    public class DomainToResourceMappingProfile : Profile
    {
        public override string ProfileName => nameof(DomainToResourceMappingProfile);

        public DomainToResourceMappingProfile()
        {
            CreateMap<Country, CountryVM>();
        }
    }
}
