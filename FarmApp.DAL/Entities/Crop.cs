using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL
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
        public int FarmId { get; set; }

        /// <summary>
        /// Id с/х культуры
        /// </summary>
        public int AgricultureId { get; set; }

        /// <summary>
        /// Урожай в тоннах
        /// </summary>
        public double Gather { get; set; }

        /// <summary>
        /// Ферма
        /// </summary>
        public virtual Farm CropFarm { get; set; }

        /// <summary>
        /// С/х культура
        /// </summary>
        public virtual Agriculture CropAgriculture { get; set; }
    }
}
