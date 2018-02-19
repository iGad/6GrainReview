using AutoMapper;
using FarmApp.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using FarmApp.ViewModels;
using System.Web.UI;
using System.Runtime.Caching;
using FarmApp.BLL.DTO;
using FarmApp.BLL.Infrastructure;
using System.Threading.Tasks;

namespace FarmApp.Controllers
{
    //я отвык видеть вообще какую-либо логику в контроллерах, т.к. мы их используем только для получения страницы (return View()) или для получения данных, которые предоставляет сервис (return service.GetSomething() или return Json(service.GetSomething()))
    //я, как человек, который привык к разделению фронта и бэка, в методах, возвращающих View не заполнял бы никакие модели, а получал их отдельным запросом с фронта, чтобы пользователь видел, что страница загрузилась, а данные еще не загрузились (если это какая-то длительная операция)
    //
    //глобальные поля не readonly и имена опять не отличаются от локальных переменных
	/// <summary>
	/// Работа с фермами
	/// </summary>
    public class FarmController : Controller
    {
		private IFarmService farmService;

		private IMapper mapper;


		public FarmController(IFarmService service, IMapper map)
			: base()
		{
			farmService = service;
			mapper = map;
		}

        //я за явное указание метода (GET, POST, UPDATE, DELETE)
        //опять непонятное приведение AsQueryable и лишний вызов ToList. Я бы написал _farmService.GetAllFarms().Select(x=>_mapper.Map<Farm,FarmViewModel>(x))
        /// <summary>
        /// Список ферм
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
			var farms = farmService.GetFarms().AsQueryable().ProjectTo<FarmViewModel>(mapper.ConfigurationProvider).ToList();
			
			return View(farms);
        }

        //честно, ни разу не использовал MemoryCache, но замысел понятен, вопрос в том как это тестировать. Я пробовал подменять статические методы через FakeReferences и Shim, но это работает только с MSTests. Обновление. Вылетело из головы. Можно написать обертку для кэша и её внедрять, тогда не будет проблем с тестированием :)
        //я бы сделал получение каждого списка отдельным запросом, чтобы страница быстрее загрузилась и отобразилась
        //хардкодные строки надо вынести в константы класса (если они только здесь используются)
        //переименовать переменные, rc - regions, fc - farmers, ac - agricultures
        //маппинг через Select(x=>_mapper.Map<>(x))
        //создать методы для cache.Get и cache.Add. в Get передавать строкой наименование и region, а возвращать или object или стразу IEnumerable<NamedItemViewModel>, в зависимости от того, что туда будем складывать. В Add передавать строкой наименование, объект и region. 
        //120 вынести в константы или настройку, можно заменить на 120000 миллисекунд
        //если оставить текущий подход (без запросов с фронта), то вынести заполнение ViewBag в отдельный метод, а получение свойств вынести с свои методы:
        /*
         private const string RegionsCacheKey = "regions";
        private const string FarmersCacheKey = "farmers";
        private const string AgricultureCacheKey = "agricultures";
        private const int CacheExpirationTimeout = 120000;

        private IEnumerable<NamedItemViewModel> GetCollectionFromCache(MemoryCache cache, string key, string regionName = null)
        {
            return cache.Get(key, regionName) as IEnumerable<NamedItemViewModel>;
        }

        private void SetDataToCache(MemoryCache cache, object data, string key, string regionName = null)
        {
            cache.Add(key, data, new DateTimeOffset(DateTime.Now.AddSeconds(CacheExpirationTimeout)));
        }

        private IEnumerable<NamedItemViewModel> GetRegions(MemoryCache cache)
        {
            var regions = GetCollectionFromCache(cache, RegionsCacheKey);
            if (regions == null)
            {
                regions = _farmService.GetRegions().Select(x => _mapper.Map<NamedItemViewModel>(x));
                SetDataToCache(cache, regions, RegionsCacheKey);
            }
            return regions;
        }

        private IEnumerable<NamedItemViewModel> GetFarmers(MemoryCache cache)
        {
            var regions = GetCollectionFromCache(cache, FarmersCacheKey);
            if (regions == null)
            {
                regions = _farmService.GetFarmers().Select(x => _mapper.Map<NamedItemViewModel>(x));
                SetDataToCache(cache, regions, FarmersCacheKey);
            }
            return regions;
        }

        private IEnumerable<NamedItemViewModel> GetAgricultures(MemoryCache cache)
        {
            var regions = GetCollectionFromCache(cache, AgricultureCacheKey);
            if (regions == null)
            {
                regions = _farmService.GetAgricultures().Select(x => _mapper.Map<NamedItemViewModel>(x));
                SetDataToCache(cache, regions, AgricultureCacheKey);
            }
            return regions;
        }
        private void FillViewBagForCreate()
        {
            MemoryCache cache = MemoryCache.Default;
            ViewBag.Regions = GetRegions(cache);
            ViewBag.Farmers = GetFarmers(cache);
            ViewBag.Agricultures = GetAgricultures(cache);
        }
             */

        /// <summary>
        /// Добавление фермы
        /// </summary>
        /// <returns></returns>        
        public ActionResult Create()
        {
            MemoryCache cache = MemoryCache.Default;
            //если есть в кэше - берём из него, в противном случае запрашиваем из IFarmService и помещаем в кэш
            var rc = cache.Get("regions") as IEnumerable<NamedItemViewModel>;
            var fc = cache.Get("farmers") as IEnumerable<NamedItemViewModel>;
            var ac = cache.Get("agricultures") as IEnumerable<NamedItemViewModel>;
            
            if(rc == null)
            {
                rc = farmService.GetRegions().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);
                cache.Add("regions", rc, new DateTimeOffset(DateTime.Now.AddSeconds(120)));
            }

            if (fc == null)
            {
                fc = farmService.GetFarmers().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);
                cache.Add("farmers", fc, new DateTimeOffset(DateTime.Now.AddSeconds(120)));
            }

            if (ac == null)
            {
                ac = farmService.GetAgricultures().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);
                cache.Add("agricultures", ac, new DateTimeOffset(DateTime.Now.AddSeconds(120)));
            }

            ViewBag.Regions = rc;
            ViewBag.Farmers = fc;
            ViewBag.Agricultures = ac;

            return View(new FarmCropViewModel());
        }

        
        //для заполнения ViewBag использовать метод, из замечания к прошлому методу.
        //делать валидацию на сервере это уже весьма устарело....но ладно, пусть будет так.
        //не понял смысла в валидации Area и Gather здесь, если это есть в AddFarmCrop, причем с другим текстом. Если уж делать валидацию через исключения, то так делать всегда.
        //я против использования исключений, которые будут очень часто возникать, т.к. это влияет на производительность, в данном случае допускаю такое использование.
        //в итоге заполнение ViewBag - 1 строка, проверка if(!ModelState.IsValid), try-catch и redirect
        [HttpPost]
        public ActionResult Create(FarmCropViewModel model)
        {
            var cache = MemoryCache.Default;
            //если есть в кэше - берём из него, в противном случае запрашиваем из IFarmService
            var rc = (cache.Get("regions") as IEnumerable<NamedItemViewModel>) ?? farmService.GetRegions().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);
            var fc = (cache.Get("farmers") as IEnumerable<NamedItemViewModel>) ?? farmService.GetFarmers().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);
            var ac = cache.Get("agricultures") as IEnumerable<NamedItemViewModel> ?? farmService.GetAgricultures().AsQueryable().ProjectTo<NamedItemViewModel>(mapper.ConfigurationProvider);
            ViewBag.Regions = rc;
            ViewBag.Farmers = fc;
            ViewBag.Agricultures = ac;

            if (model.Area <= 0)
            {
                ModelState.AddModelError("Area", "Площадь должна быть больше нуля");
            }

            if(model.Gather < 0)
            {
                ModelState.AddModelError("Gather", "Урожай не может быть меньше нуля");
            }

            if(!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var farmCrop = mapper.Map<FarmCropDto>(model);
                farmService.AddFarmCrop(farmCrop);
            }
            catch(ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return View(model);
            }

            return RedirectToAction("List");
        }

        //переименовать Id в id
        /// <summary>
        /// Удаление фермы
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ActionResult Delete(int Id)
        {
            var farm = farmService.GetFarm(Id);

            var farmModel = mapper.Map<FarmViewModel>(farm);

            return View(farmModel);
        }

        //переименовать Id в id
        //
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, FormCollection formCollection)
        {
            farmService.DeleteFarm(Id);

            return RedirectToAction("List");
        }
    }
}
