using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL
{
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
        public virtual List<Crop> Crops { get; set; }

        public Agriculture()
        {
            Crops = new List<Crop>();
        }
    }
}
