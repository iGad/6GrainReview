using AutoMapper;
using FarmApp.BLL.DTO;
using FarmApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.BLL
{
    public class AutoMapperConfig
    {
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<FarmCropDto, Farm>();
                cfg.CreateMap<FarmCropDto, Crop>();
                cfg.CreateMap<Agriculture, AgricultureDto>();
                cfg.CreateMap<Farmer, FarmerDto>();
                cfg.CreateMap<Region, RegionDto>();
            });
            return new Mapper(config);
        }
    }
}
