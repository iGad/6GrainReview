using AutoMapper;
using FarmApp.BLL.DTO;
using FarmApp.DAL;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.BLL.Tests
{
    [TestFixture]
    public class AutoMapperConfigTests
    {
        private IMapper mapper;

        [SetUp]
        public void Configure()
        {
            mapper = AutoMapperConfig.GetMapper();
        }

        [Test]
        public void Mapping_FarmCropDtoToFarm_IsValid()
        {
            var from = new FarmCropDto() { AgricultureId = 1, Area = 2, FarmerId = 3, Gather = 4, Name = "abc", RegionId = 5 };
            var to = mapper.Map<FarmCropDto, Farm>(from);

            Assert.IsTrue(to.Name == "abc" && to.Area == 2 && to.FarmerId == 3 && to.RegionId == 5);
        }

        [Test]
        public void Mapping_FarmCropDtoToCrop_IsValid()
        {
            var from = new FarmCropDto() { AgricultureId = 1, Area = 2, FarmerId = 3, Gather = 4, Name = "abc", RegionId = 5 };
            var to = mapper.Map<FarmCropDto, Crop>(from);

            Assert.IsTrue(to.AgricultureId == 1 && to.Gather == 4);
        }

        [Test]
        public void Mapping_RegionToRegionDto_IsValid()
        {
            var from = new RegionDto() { Id = 1, Name = "abc" };
            var to = mapper.Map<RegionDto, Region>(from);

            Assert.IsTrue(to.Id == 1 && to.Name == "abc");
        }
    }
}
