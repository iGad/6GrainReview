using AutoMapper;
using FarmApp.BLL.DTO;
using FarmApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmApp.Util
{
    public class AutoMapperConfig
    {
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FarmDto, FarmViewModel>()
                    .ForMember(des => des.FarmerName, opt => opt.MapFrom(src => src.OwnerName))
                    .ForMember(des => des.RegionName, opt => opt.MapFrom(src => src.LocationName));
                cfg.CreateMap<RegionDto, NamedItemViewModel>();
                cfg.CreateMap<FarmerDto, NamedItemViewModel>();
                cfg.CreateMap<AgricultureDto, NamedItemViewModel>();
                cfg.CreateMap<FarmCropViewModel, FarmCropDto>();

            });
            return new Mapper(config);
        }
    }
}