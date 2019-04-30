using AutoMapper;
using MyRestful.Api.ViewModel;
using MyRestful.Core;
using MyRestful.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRestful.Api.Configuration
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName => nameof(ViewModelToDomainMappingProfile);
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<CountryVM, Country>();
        }
    }
}
