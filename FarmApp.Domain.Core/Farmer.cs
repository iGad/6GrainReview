using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.Domain.Core
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
        public virtual List<Farm> Farms { get; set; }

        public Farmer()
        {
            Farms = new List<Farm>();
        }
    }
}
