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
    //нужно объединить с AutoMapperConfig из FarmApp.Web, например здесь принимать в качестве параметра IMapperConfigurationExpression и его же возвращать
    public class AutoMapperConfig
    {
        public static Mapper GetMapper()
        {
            //не хватает cfg.CreateMap<Farm, FarmDto>(); и cfg.CreateMap<RegionDto, Region>(); но видимо всё и без этого работает (как оказалось, не всё работает)
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
