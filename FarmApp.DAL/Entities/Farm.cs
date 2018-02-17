using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL //не соответствует фактическому расположению, должно быть FarmApp.DAL.Entities
{
    /// <summary>
    /// Ферма
    /// </summary>
    public class Farm
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Площадь
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// Id фермера
        /// </summary>
        public int FarmerId { get; set; } //Не соответствует конвенции о наименовании. Может запутать. Либо переименовать в OwnerId или Owner переименовать в Farmer (я за второй вариант)

        /// <summary>
        /// Id региона
        /// </summary>
        public int RegionId { get; set; }//Не соответствует конвенции о наименовании. Может запутать. Либо переименовать в LocationId или Location переименовать в Region (я за второй вариант)

        /// <summary>
        /// Фермер-владелец
        /// </summary>
        public virtual Farmer Owner { get; set; }//я против неявной ленивой загрузки, т.к. можно легким движением серьезно ухудшить производительность. Получение значения свойства не должно занимать неопределенное время.

        /// <summary>
        /// Регион-расположение
        /// </summary>
        public virtual Region Location { get; set; }//я против неявной ленивой загрузки, т.к. можно легким движением серьезно ухудшить производительность. Получение значения свойства не должно занимать неопределенное время.
    }
}
