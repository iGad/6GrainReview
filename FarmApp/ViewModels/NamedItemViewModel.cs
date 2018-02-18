using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarmApp.ViewModels
{
    //я бы добавил summary, что это класс для отображения "справочных" сущностей
    public class NamedItemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}