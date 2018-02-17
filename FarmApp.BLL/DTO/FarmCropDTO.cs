using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.BLL.DTO
{
    /// <summary>
    /// Информация по урожаю на ферме
    /// </summary>
    public class FarmCropDto
    {
        public string Name { get; set; }

        public int FarmerId { get; set; }

        public int RegionId { get; set; }

        public int AgricultureId { get; set; }

        public double Area { get; set; }

        public double Gather { get; set; }
    }
}
