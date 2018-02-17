using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL //не соответствует фактическому расположению, должно быть FarmApp.DAL.Entities
{
    /// <summary>
    /// Урожай
    /// </summary>
    public class Crop
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id фермы
        /// </summary>
        public int FarmId { get; set; }//Не соответствует конвенции о наименовании. Может запутать. Либо переименовать в CropFarmId или CropFarm переименовать в Farm (я за второй вариант)

        /// <summary>
        /// Id с/х культуры
        /// </summary>
        public int AgricultureId { get; set; }//Не соответствует конвенции о наименовании. Может запутать. Либо переименовать в CropAgricultureId или CropAgriculture переименовать в Agriculture (я за второй вариант)

        /// <summary>
        /// Урожай в тоннах
        /// </summary>
        public double Gather { get; set; }

        /// <summary>
        /// Ферма
        /// </summary>
        public virtual Farm CropFarm { get; set; } //я против неявной ленивой загрузки, т.к. можно легким движением серьезно ухудшить производительность. Получение значения свойства не должно занимать неопределенное время.

        /// <summary>
        /// С/х культура
        /// </summary>
        public virtual Agriculture CropAgriculture { get; set; } //я против неявной ленивой загрузки, т.к. можно легким движением серьезно ухудшить производительность. Получение значения свойства не должно занимать неопределенное время.
    }
}
