using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL.Interfaces
{
    //Мне не нравится данный подход тем, что по факту это обертка над DbContext, который сам по себе является UnitOfWork. Более того, если нужен сервис, 
    //который работает только с одним репозиторием, то в него всё равно придется внедрять весь IUnitOfWork и сервис может делать всё, что угодно со всей БД.
    //В тестах, которые будут покрывать код, зависящий от IUnitOfWork, придется делать как заглушку для IUnitOfWork, так и для каждого из свойств-репозиториев, т.к. сам IUnitOfWork по факту просто обединяет кучу зависимостей
    /// <summary>
    /// Интерфейс паттерна Unit of work
    /// </summary>
	public interface IUnitOfWork : IDisposable
	{
		IRepository<Agriculture> Agricultures { get; }

		IRepository<Crop> Crops { get; }

		IRepository<Farm> Farms { get; }

		IRepository<Farmer> Farmers { get; }

		IRepository<Region> Regions { get; }

		void Save();
	}
}
