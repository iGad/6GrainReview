using FarmApp.BLL.DTO;
using FarmApp.BLL.Infrastructure;
using FarmApp.BLL.Services;
using FarmApp.DAL;
using FarmApp.DAL.Interfaces;
using Moq;
using NUnit.Framework;

namespace FarmApp.BLL.Tests
{
    //Опять глобальная переменная. SetUp метод есть смысл применять для интеграционных тестов.
    //Вместо try-catch-assert надо использовать Assert.Catch<ValidationException>
    //Я бы вместо мок-объекта для репозитория использовал бы тестовый Generic-репозиторий на основе списка. Да, использовать список вместо DbSet это не идеальный вариант, но это куда удобнее:
    //В тестах можно привести IRepository к TestRepository, у которого сделать открытым внутренний список, чтобы можно было без вызова Get или FindById проверить выполнилось ли добавление и узнать значения свойств, плюс, можно добавить свойство SaveChangesCallCount, чтобы убедиться в том, что метод SaveChanges вызывался
    [TestFixture]
    public class FarmerServiceTests
    {
        private FarmService farmService;

        [SetUp]
        public void Configure()
        {
            //о чем я и писал в IUnitOfWork - чтобы замокать IUnitOfWork надо сделать мок для свойств
            var cropRepo = new Mock<IRepository<Crop>>();
            cropRepo.Setup(item => item.Create(It.IsAny<Crop>())).Callback(() => { });
            var iow = new Mock<IUnitOfWork>();
            iow.Setup(item => item.Crops).Returns(cropRepo.Object);
            farmService = new FarmService(iow.Object, AutoMapperConfig.GetMapper());
        }

        //я бы назвал AddFarmCrop_WhenFarmCropAgricultureIdIsInvalid_ThrowValidationException
        /*
            [Test]
            public void AddFarmCrop_WhenFarmCropAgricultureIdIsInvalid_ThrowValidationException()
            {
                var service = CreateService();
                FarmCropDto farmCropDto = new FarmCropDto { AgricultureId = -1, FarmerId = 1, RegionId = 1, Area = 1, Gather = 1, Name = "abc" };
                
                Assert.Catch<ValidationException>(() => service.AddFarmCrop(farmCropDto));
            }
             */
        [Test]
        public void AddFarmCrop_InvalidAgricultureId_ThrowValidationException()
        {
            FarmCropDto enemy = new FarmCropDto() { AgricultureId = -1, FarmerId = 1, RegionId = 1, Area = 1, Gather = 1, Name = "abc" };
            try
            {
                farmService.AddFarmCrop(enemy);
            }
            catch(ValidationException ex)
            {
                Assert.IsTrue(ex.Property == "AgricultureId");
            }
        }

        [Test]
        public void AddFarmCrop_InvalidFarmerId_ThrowValidationException()
        {            
            FarmCropDto enemy = new FarmCropDto() { AgricultureId = 1, FarmerId = -1, RegionId = 1, Area = 1, Gather = 1, Name = "abc" };
            try
            {
                farmService.AddFarmCrop(enemy);
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.Property == "FarmerId");
            }
        }

        [Test]
        public void AddFarmCrop_InvalidRegionId_ThrowValidationException()
        {            
            FarmCropDto enemy = new FarmCropDto() { AgricultureId = 1, FarmerId = 1, RegionId = -1, Area = 1, Gather = 1, Name = "abc" };
            try
            {
                farmService.AddFarmCrop(enemy);
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.Property == "RegionId");
            }
        }

        [Test]
        public void AddFarmCrop_InvalidName_ThrowValidationException()
        {
            var cropRepo = new Mock<IRepository<Crop>>();
            cropRepo.Setup(item => item.Create(It.IsAny<Crop>())).Callback(() => { });
            var iow = new Mock<IUnitOfWork>();
            iow.Setup(item => item.Crops).Returns(cropRepo.Object);

            var farmService = new FarmService(iow.Object, AutoMapperConfig.GetMapper());
            FarmCropDto enemy = new FarmCropDto() { AgricultureId = 1, FarmerId = 1, RegionId = 1, Area = 1, Gather = 1, Name = "" };
            try
            {
                farmService.AddFarmCrop(enemy);
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.Property == "Name");
            }
        }

        [Test]
        public void AddFarmCrop_InvalidGather_ThrowValidationException()
        {
            var cropRepo = new Mock<IRepository<Crop>>();
            cropRepo.Setup(item => item.Create(It.IsAny<Crop>())).Callback(() => { });
            var iow = new Mock<IUnitOfWork>();
            iow.Setup(item => item.Crops).Returns(cropRepo.Object);

            var farmService = new FarmService(iow.Object, AutoMapperConfig.GetMapper());
            FarmCropDto enemy = new FarmCropDto() { AgricultureId = 1, FarmerId = 1, RegionId = 1, Area = 1, Gather = -1, Name = "abc" };
            try
            {
                farmService.AddFarmCrop(enemy);
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.Property == "Gather");
            }
        }

        [Test]
        public void AddFarmCrop_InvalidArea_ThrowValidationException()
        {
            var cropRepo = new Mock<IRepository<Crop>>();
            cropRepo.Setup(item => item.Create(It.IsAny<Crop>())).Callback(() => { });
            var iow = new Mock<IUnitOfWork>();
            iow.Setup(item => item.Crops).Returns(cropRepo.Object);

            var farmService = new FarmService(iow.Object, AutoMapperConfig.GetMapper());
            FarmCropDto enemy = new FarmCropDto() { AgricultureId = 1, FarmerId = 1, RegionId = 1, Area = 0, Gather = 1, Name = "abc" };
            try
            {
                farmService.AddFarmCrop(enemy);
            }
            catch (ValidationException ex)
            {
                Assert.IsTrue(ex.Property == "Area");
            }
        }

        //Если сделать так, как я описал в шапке, то тесты корректной работы метода AddFarmCrop выглядели бы примерно так
        /*
            [Test]
            public void AddFarmCrop_WhenFarmCropValid_IncreaseTotalFarmCount()
            {
                var farmRepo = CreateFarmRepository();//возвращает TestRepository<Farm> : IRepository<Farm>
                var oldCount = farmRepo.List.Count;//List это список, имитирующий таблицу
                var service = CreateService(farmRepo);//В конструктор передается конкретно этот репозиторий
                var farmCropDto = CreateValidFarmCropDto();

                service.AddFarmCrop(farmCropDto);

                Assert.AreEqual(oldCount + 1, farmRepo.List.Count);
            }

            [Test]
            [TestCase(nameof(Farm.Area))]
            [TestCase(nameof(Farm.RegionId))]
            [TestCase(nameof(Farm.FarmerId))]
            [TestCase(nameof(Farm.Name))]
            public void AddFarmCrop_WhenFarmCropValid_AddFarmWithExpectedProperties(string propertyName)
            {
                var farmRepo = CreateFarmRepository();
                var oldCount = farmRepo.List.Count;
                var service = CreateService(farmRepo);
                var farmCropDto = CreateValidFarmCropDto();

                service.AddFarmCrop(farmCropDto);

                var farm = farmRepo.List.Last();
                Assert.That(farm, Has.Property(propertyName).EqualTo(GetValue(farmCropDto, propertyName)));            
            }
             */
        [Test]
        public void AddFarmCrop_ValidateModelMappingWhenSave_IsValid_()
        {
            bool isValid = false;
            var cropRepo = new Mock<IRepository<Crop>>();
            cropRepo.Setup(item => item.Create(It.IsAny<Crop>())).Callback<Crop>(arg =>
            {
                isValid = arg.AgricultureId == 1 && arg.Gather == 5 && arg.CropFarm.Name == "abc" && arg.CropFarm.FarmerId == 2 && arg.CropFarm.RegionId == 3;
            });
            var iow = new Mock<IUnitOfWork>();
            iow.Setup(item => item.Crops).Returns(cropRepo.Object);
            farmService = new FarmService(iow.Object, AutoMapperConfig.GetMapper());
            FarmCropDto enemy = new FarmCropDto() { AgricultureId = 1, FarmerId = 2, RegionId = 3, Area = 4, Gather = 5, Name = "abc" };
            farmService.AddFarmCrop(enemy);
            Assert.IsTrue(isValid);
        }

    }
}
