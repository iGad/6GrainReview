using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.BLL.DTO
{
    /// <summary>
    /// Информация о ферме
    /// </summary>
    public class FarmDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int FarmerId { get; set; }

        public string OwnerName { get; set; }

        public int RegionId { get; set; }

        public string LocationName { get; set; }

        public double Area { get; set; }
    }
}
