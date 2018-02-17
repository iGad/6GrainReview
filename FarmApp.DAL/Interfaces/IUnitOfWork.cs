using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL.Interfaces
{
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
