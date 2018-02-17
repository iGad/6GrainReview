using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmApp.ViewModels
{
	public class FarmViewModel
	{
		public int Id { get; set; }

        [Display(Name = "Наименование")]
        public string Name { get; set; }

		public int FarmerId { get; set; }

        [Display(Name = "Имя фермера")]
        public string FarmerName { get; set; }

		public int RegionId { get; set; }

        [Display(Name = "Регион")]
        public string RegionName { get; set; }

        [Display(Name = "Площадь")]
        public double Area { get; set; }
	}
}