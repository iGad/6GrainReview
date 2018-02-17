using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL //не соответствует фактическому расположению, должно быть FarmApp.DAL.Entities
{
    //Все сущности стоит унаследовать от одного базового класса со свойством [Key] int Id {get;set;}, чтобы репозитории имели ограничение на класс
    //Использование в качестве ключа int имеет свои достоинства и недостатки перед string и Guid, так что тут всё зависит от того, как принято в компании
    /// <summary>
    /// С/х культура
    /// </summary>
    public class Agriculture
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
        /// Урожаи
        /// </summary>
        public virtual List<Crop> Crops { get; set; }//я против неявной ленивой загрузки, т.к. можно легким движением серьезно ухудшить производительность. Получение значения свойства не должно занимать неопределенное время.

        public Agriculture()
        {
            Crops = new List<Crop>();//удалить
        }
    }
}
