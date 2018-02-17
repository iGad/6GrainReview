using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FarmApp.BLL.DTO;
using FarmApp.BLL.Infrastructure;
using FarmApp.BLL.Interfaces;
using FarmApp.DAL;
using FarmApp.DAL.Interfaces;

namespace FarmApp.BLL.Services
{
	/// <summary>
	/// Реализует операции бизнес-логики
	/// </summary>
    public class FarmService : IFarmService
    {
        private IUnitOfWork database;

		private IMapper mapper;

        public FarmService(IUnitOfWork uow, IMapper map)
        {
			database = uow ?? throw new ArgumentNullException("uow");			
			mapper = map ?? throw new ArgumentNullException("map");
		}

        public void AddFarmCrop(FarmCropDto farmCrop)
        {
            if (farmCrop == null)
                throw new ValidationException($"Параметр farmCorp равен null", "");

            if (farmCrop.AgricultureId < 1)
                throw new ValidationException($"Неправильное значение: {farmCrop.AgricultureId }", "AgricultureId");

            if (farmCrop.FarmerId < 1)
                throw new ValidationException($"Неправильное значение: {farmCrop.FarmerId }", "FarmerId");

            if (farmCrop.RegionId < 1)
                throw new ValidationException($"Неправильное значение: {farmCrop.RegionId }", "RegionId");

            if (string.IsNullOrEmpty(farmCrop.Name))
                throw new ValidationException($"Не задано имя фермы", "Name");

            if (farmCrop.Gather < 0)
                throw new ValidationException($"Значение урожайности не может быть отрицательным", "Gather");

            if (farmCrop.Area <= 0)
                throw new ValidationException($"Значение площади должно быть больше нуля", "Area");

            try
            {

                var farm = mapper.Map<FarmCropDto, Farm>(farmCrop);

                var crop = mapper.Map<FarmCropDto, Crop>(farmCrop);

                crop.CropFarm = farm;                
                database.Crops.Create(crop);
                database.Save();
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка сохранения информации об урожае на ферме", ex);
            }

        }

        public void DeleteFarm(int Id)
        {
            
            var farmToRemove = database.Farms.Get(f => f.Id == Id).FirstOrDefault();
            if (farmToRemove == null)
                throw new Exception($"Ферма с Id {Id} не найдена");
            try
            {
                database.Farms.Remove(farmToRemove);
                database.Save();
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка удаления фермы", ex);
            }
        }

        public IEnumerable<AgricultureDto> GetAgricultures()
        {
            return database.Agricultures.Get().AsQueryable().ProjectTo<AgricultureDto>(mapper.ConfigurationProvider).ToList();
        }

        public IEnumerable<FarmerDto> GetFarmers()
        {
            return database.Farmers.Get().AsQueryable().ProjectTo<FarmerDto>(mapper.ConfigurationProvider).ToList();
        }

        public IEnumerable<FarmDto> GetFarms()
        {
            return database.Farms.Get().AsQueryable().ProjectTo<FarmDto>(mapper.ConfigurationProvider).ToList();
        }

        public FarmDto GetFarm(int Id)
        {
            var farm = database.Farms.Get(f => f.Id == Id).FirstOrDefault();
            return mapper.Map<FarmDto>(farm);
        }

        public IEnumerable<RegionDto> GetRegions()
        {
			return database.Regions.Get().AsQueryable().ProjectTo<RegionDto>(mapper.ConfigurationProvider).ToList();
        }

		private bool disposed = false;

		public void Dispose()
        {
			Dispose(true);
			GC.SuppressFinalize(this);
			
        }
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// Освобождаем управляемые ресурсы
					database.Dispose();
					database = null;
					mapper = null;					
				}
				disposed = true;
			}
		}	
	}
}
