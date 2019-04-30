using AutoMapper;
using MyRestful.Api.Models;
using MyRestful.Api.ViewModel;
using MyRestful.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestful.Api.Configuration
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
