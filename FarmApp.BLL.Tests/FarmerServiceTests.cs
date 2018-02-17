using FarmApp.BLL.DTO;
using FarmApp.BLL.Infrastructure;
using FarmApp.BLL.Services;
using FarmApp.DAL;
using FarmApp.DAL.Interfaces;
using Moq;
using NUnit.Framework;

namespace FarmApp.BLL.Tests
{
    [TestFixture]
    public class FarmerServiceTests
    {
        private FarmService farmService;

        [SetUp]
        public void Configure()
        {
            var cropRepo = new Mock<IRepository<Crop>>();
            cropRepo.Setup(item => item.Create(It.IsAny<Crop>())).Callback(() => { });
            var iow = new Mock<IUnitOfWork>();
            iow.Setup(item => item.Crops).Returns(cropRepo.Object);
            farmService = new FarmService(iow.Object, AutoMapperConfig.GetMapper());
        }

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
