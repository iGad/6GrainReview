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
    //1. Я бы не использовал глобальную не readonly переменную. Тесты должны быть независимы и обособлены друг от друга. В глобальной переменной можно хранить какой-нибудь массив или строку, которая используется тольок для чтения.
    //2. Вместо Configure в каждом тесте вызывал бы создание маппера (сделал бы метод CreateMapper)
    //3. Дал бы другие имена тестам, чтобы сначала шло название метода, потом условие, а потом ожидаемый результат, например Map_FromFarmToFarmDto_SetExpected<имя свойства>
    //4. Сделал бы тесты параметризованными, это позволяет не писать большие конструкции в Assert, по которым по сообщению об ошибке не будет понятно в чем дело, и не надо будет для каждого свойства писать тест(ы), достаточно добавить параметр
    /*
        private IMapper CreateMapper()
        {
            return AutoMapperConfig.GetMapper();
        }

        private Farm CreateFarm()
        {
            return new Farm { Area = 2, FarmerId = 3, Name = "farm", RegionId = 5 };
        }

        private void SetValue(object entity, string propertyName, object value)
        {
            entity.GetType().GetProperty(propertyName).SetValue(entity, value);
        }

        [Test]
        [TestCase(nameof(Farm.Area), 20.456)]
        [TestCase(nameof(Farm.FarmerId), 2)]
        [TestCase(nameof(Farm.Name), "test farm")]
        [TestCase(nameof(Farm.RegionId), 3)]
        public void Map_FromFarmToFarmDto_SetExpectedProperty(string propertyName, object expectedValue)
        {
            //Arrange
            var mapper = CreateMapper();
            var farm = CreateFarm();
            SetValue(farm, propertyName,  expectedValue);

            //Act
            var farmDto = mapper.Map<Farm, FarmDto>(farm);

            //Assert
            Assert.That(farmDto, Has.Property(propertyName).EqualTo(expectedValue));
        }
         */
    [TestFixture]
    public class AutoMapperConfigTests
    {
        private IMapper mapper;

        [SetUp]
        public void Configure()
        {
            mapper = AutoMapperConfig.GetMapper();
        }

        private IMapper CreateMapper()
        {
            return AutoMapperConfig.GetMapper();
        }

        private Farm CreateFarm()
        {
            return new Farm { Area = 2, FarmerId = 3, Name = "farm", RegionId = 5 };
        }

        private void SetValue(object entity, string propertyName, object value)
        {
            entity.GetType().GetProperty(propertyName).SetValue(entity, value);
        }

        [Test]
        [TestCase(nameof(Farm.Area), 20.456)]
        [TestCase(nameof(Farm.FarmerId), 2)]
        [TestCase(nameof(Farm.Name), "test farm")]
        [TestCase(nameof(Farm.RegionId), 3)]
        public void Map_FromFarmToFarmDto_SetExpectedProperty(string propertyName, object expectedValue)
        {
            var mapper = CreateMapper();
            var farm = CreateFarm();
            SetValue(farm, propertyName,  expectedValue);

            var farmDto = mapper.Map<Farm, FarmDto>(farm);

            Assert.That(farmDto, Has.Property(propertyName).EqualTo(expectedValue));
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

        //тест не проходит, т.к. нет в конфигурации маппинга для Map<RegionDto, Region>
        [Test]
        public void Mapping_RegionToRegionDto_IsValid()
        {
            var from = new RegionDto() { Id = 1, Name = "abc" };
            var to = mapper.Map<RegionDto, Region>(from);

            Assert.IsTrue(to.Id == 1 && to.Name == "abc");
        }
    }
}
