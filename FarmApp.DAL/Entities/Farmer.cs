using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL //не соответствует фактическому расположению, должно быть FarmApp.DAL.Entities
{
    /// <summary>
    /// Фермер
    /// </summary>
    public class Farmer
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
        /// Фермы
        /// </summary>
        public virtual List<Farm> Farms { get; set; } //я против неявной ленивой загрузки, т.к. можно легким движением серьезно ухудшить производительность. Получение значения свойства не должно занимать неопределенное время.

        public Farmer()
        {
            Farms = new List<Farm>();
        }
    }
}
