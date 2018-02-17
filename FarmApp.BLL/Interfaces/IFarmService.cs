using FarmApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.BLL.Interfaces
{
    /// <summary>
    /// Предоставляет набор операций бизнес-логики
    /// </summary>
    public interface IFarmService : IDisposable
    {
        /// <summary>
        /// Получить информацию по фермам
        /// </summary>
        /// <returns></returns>
        IEnumerable<FarmDto> GetFarms();


        /// <summary>
        /// Получить информацию по ферме с заданным Id
        /// </summary>
        /// <param name="Id">Id фермы</param>
        /// <returns></returns>
        FarmDto GetFarm(int Id);

        /// <summary>
        /// Получить информацию по фермерам
        /// </summary>
        /// <returns></returns>
        IEnumerable<FarmerDto> GetFarmers();

        /// <summary>
        /// Получить информацию по регионам
        /// </summary>
        /// <returns></returns>
        IEnumerable<RegionDto> GetRegions();

        /// <summary>
        /// Получить информацию по с/х культурам
        /// </summary>
        /// <returns></returns>
        IEnumerable<AgricultureDto> GetAgricultures();

        /// <summary>
        /// Удалить ферму
        /// </summary>
        /// <param name="Id">Идентификатор удаляемой фермы</param>
        void DeleteFarm(int Id);

        /// <summary>
        /// Добавление сведений ферме и урожае на ней
        /// </summary>
        /// <param name="farmCrop"></param>
        void AddFarmCrop(FarmCropDto farmCrop);
    }
}
