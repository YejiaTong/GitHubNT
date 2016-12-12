using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using MVCWebApp.UIClasses;
using MVCWebApp.Models.Home;

namespace MVCWebApp
{
    /*
     * Global AutoMapper for class - view model mapping
     * In support of front-end presentment
     * 
     * Noah Tong - Dec 09, 2016
     * */
    public static class MapperFactory
    {
        public static MapperConfiguration NETDateViewModel_UINETDate;

        public static void InitializeAutoMapperFactory()
        {
            try
            {
                // Mapping setup
                NETDateViewModel_UINETDate = new MapperConfiguration(cfg => cfg.CreateMap<NETDateViewModel, UINETDate>()
                    .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year))
                    .ForMember(dest => dest.Month, opt => opt.MapFrom(src => src.Month))
                    .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.Day))
                    .ForMember(dest => dest.Data, opt => opt.Ignore())
                    .ReverseMap());

                // Validate all mappings
                NETDateViewModel_UINETDate.AssertConfigurationIsValid();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
