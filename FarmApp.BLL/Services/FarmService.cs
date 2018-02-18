using System;
using System.Collections.Generic;
using System.Globalization;
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
    //лично мне непонятен смысл использования в этом задании DTO объектов для сущностей БД. 
    //Разве что человек пытался показать, что он знает что это такое и прочитал, что веб-проект не должен ссылаться на проект с сущностями БД, ведь, якобы, может случиться так, что мы поменяем слой работы с БД и тогда не придется переписывать слой бизнес-логики....ну в книжке да, в теории всё красиво :)
    //по-моему, если dto объекты не содержат какой-либо логики, а просто копируют свойства сущностей БД, то маппинг - это лишняя нагрузка, но если в команде принято использование dto, то ок.
    //опять глобальные поля без _ и не readonly
    //database название этой переменной меня смущает, вроде работа с абстракцией абстракций, а переменная называется конкретно :) я бы назвал unitOfWork (хотя я бы вообще его не использовал, а внедрил несколько IRepository<>)

    //смущает наличие методов GetFarmers, GetAgricultures и GetRegions. Поскольку это тестовое задание, в котором очень ограниченный функционал, то я допускаю, что эти методы могут быть здесь,
    //но в реальном проекте, было бы 3 сервиса (по каждому для фермеров, культур и регионов), в которых был бы CRUD для соответствующих сущностей (а может быть и один GenericService, т.к. это сущности-справочники и логика CRUD для них одинакова, по крайней мере в такой реализации)
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

        //Нет описания метода (summary)
        //В задании не сказано про уникальность имен ферм, можно допустить, что это неважно
        //1. Хардкодные и повторяющиеся строки, нужно использовать ресурсы или константы
        //2. Передача в качестве строки имени свойства, когда есть nameof(), которому не страшно переименование
        //3. throw new Exception - лучше использовать собственный тип исключений, чтобы в случае надобности можно было по нему сделать фильтрацию
        //4. catch(Exception ex) - лучше использовать глобальный обработчик исключений, а текст сообщения об ошибке запроса писать на фронтэнде, т.к. там известно что за запрос был и известно, что он не выполнился. Более того, при выбросе нового исключения затрется StackTrace исходного исключения, а это важная информация
        public void AddFarmCrop(FarmCropDto farmCrop)
        {
            if (farmCrop == null)
                throw new ValidationException($"Параметр farmCorp равен null", "");
            //тут два варианта: 1. создать метод ValidateFarmCrop или, раз уж есть DTO, в FarmCropDto создать метод Validate, в который поместить if'ы, а здесь его вызвать
            
            //интересная проверка связанных идентификаторов....вообще, и без этой проверки при сохранении возникнет исключение, и эта проверка не поможет, если связанной сущности с указанным идентификатором нет.
            //Я бы удалил первые 3 if'a
            if (farmCrop.AgricultureId < 1)
                throw new ValidationException($"Неправильное значение: {farmCrop.AgricultureId }", "AgricultureId");
            if (farmCrop.FarmerId < 1)
                throw new ValidationException($"Неправильное значение: {farmCrop.FarmerId }", "FarmerId");
            if (farmCrop.RegionId < 1)
                throw new ValidationException($"Неправильное значение: {farmCrop.RegionId }", "RegionId");

            //заменить IsNullOrEmpty на IsNullOrWhiteSpace
            //эти if'ы вынести в метод, про который я выше написал
            if (string.IsNullOrEmpty(farmCrop.Name))
                throw new ValidationException($"Не задано имя фермы", "Name");

            if (farmCrop.Gather < 0)
                throw new ValidationException($"Значение урожайности не может быть отрицательным", "Gather");

            if (farmCrop.Area <= 0)
                throw new ValidationException($"Значение площади должно быть больше нуля", "Area");
            //try-catch убрать
            try
            {

                var farm = mapper.Map<FarmCropDto, Farm>(farmCrop);

                var crop = mapper.Map<FarmCropDto, Crop>(farmCrop);
                //в EF такой код сработает как нужно, а с другой ORM? 
                //я бы сначала добавил farm, потом crop.FarmId = farm.Id; и потом добавил crop
                crop.CropFarm = farm;                
                database.Crops.Create(crop);
                database.Save();
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка сохранения информации об урожае на ферме", ex);
            }

        }

        //Нет summary
        //параметр почему-то с большой буквы, нужно с маленькой
        //зачем использовать метод Get(f => f.Id == Id) если есть FindById
        //try-catch убрать
        //throw new Exception заменить на свой тип исключения
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
        //замечание в шапке и в GetFarms
        public IEnumerable<AgricultureDto> GetAgricultures()
        {
            return database.Agricultures.Get().AsQueryable().ProjectTo<AgricultureDto>(mapper.ConfigurationProvider).ToList();
        }
        //замечание в шапке и в GetFarms
        public IEnumerable<FarmerDto> GetFarmers()
        {
            return database.Farmers.Get().AsQueryable().ProjectTo<FarmerDto>(mapper.ConfigurationProvider).ToList();
        }

        //эти замечания относятся ко всем методам получения данных
        //AsQueryable() только ради того, чтобы вызвать ProjectTo, т.к. репозиторий возвращает IEnumerable и все данные уже в памяти. Я бы вообще никакого маппинга не делал, 
        //но если решили использовать DTO, то написал бы _farmsRepository.Get(x=>true).Select(x=>_mapper.Map<Farm, FarmDto>(x));
        //не за чем вызывать ToList, т.к. и так все данные в памяти
        public IEnumerable<FarmDto> GetFarms()
        {
            return database.Farms.Get().AsQueryable().ProjectTo<FarmDto>(mapper.ConfigurationProvider).ToList();
        }

        //Нет summary
        //параметр почему-то с большой буквы, нужно с маленькой
        //зачем использовать метод Get(f => f.Id == Id) если есть FindById
        //нет обработки случая, когда сущность не найдена (по-моему должно быть исключение)
        public FarmDto GetFarm(int Id)
        {
            var farm = database.Farms.Get(f => f.Id == Id).FirstOrDefault();
            return mapper.Map<FarmDto>(farm);
        }
        //замечание в шапке и в GetFarms
        public IEnumerable<RegionDto> GetRegions()
        {
			return database.Regions.Get().AsQueryable().ProjectTo<RegionDto>(mapper.ConfigurationProvider).ToList();
        }

        //реализацию IDisposable убрать
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
