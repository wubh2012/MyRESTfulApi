using AutoMapper;
using MyRESTfulApi.Models;
using MyRESTfulApi.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyRESTfulApi.Configuration
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
