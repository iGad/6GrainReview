using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL.Interfaces
{
    /// <summary>
    /// Обобщённый репозиторий - набор операций над сущностью
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
	public interface IRepository<TEntity> where TEntity : class
	{
		void Create(TEntity item);

		TEntity FindById(int id);

		IEnumerable<TEntity> Get();

		IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);

		void Remove(TEntity item);

		void Update(TEntity item);
	}
}
