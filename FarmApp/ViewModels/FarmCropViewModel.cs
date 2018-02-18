using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FarmApp.ViewModels
{
    //Хардкодные строки заменить на ресурсы
    //не понял почему вдруг переменные значимого типа стали Nullable и при этом Required. Может я чего-то не знаю или не помню, я валидацию на основе атрибутов очень давно не использовал
    //
    public class FarmCropViewModel
    {
        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Поле должно быть заполнено", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = "Фермер")]
        [Required(ErrorMessage = "Нужно выбрать фермера")]        
        public int? FarmerId { get; set; }

        [Display(Name = "Регион")]
        [Required(ErrorMessage = "Нужно выбрать регион")]
        public int? RegionId { get; set; }

        [Display(Name = "C/х культура")]
        [Required(ErrorMessage = "Нужно выбрать с/х культуру")]
        public int? AgricultureId { get; set; }
                
        [Display(Name = "Площадь")]
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public double? Area { get; set; }

        [Display(Name = "Урожай за прошлый год")]        
        [Required(ErrorMessage = "Поле должно быть заполнено")]
        public double? Gather { get; set; }
    }
}