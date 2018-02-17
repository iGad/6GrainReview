using FarmApp.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FarmApp.DAL.Repositories
{
    //все замечания в IRepository<TEntity> актульны и тут
    //EFRepository стоит переименовать, например, в GenericRepository. В наименовании нежелательно использовать аббревиатуры
    //здесь почему-то глобальные поля называются с _, а в EFUnitOfWork без _. Надо привести к единому стилю
    //_context и _dbSet надо сделать readonly.
    /// <summary>
    /// Реализация репозитория в контексте EF
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
	public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		DbContext _context;
		DbSet<TEntity> _dbSet;

		public EFRepository(DbContext context)
		{
			_context = context;
			_dbSet = context.Set<TEntity>();
		}

		public IEnumerable<TEntity> Get()
		{
			return _dbSet;
		}

		public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
		{
			return _dbSet.Where(predicate);//в таком случае Where это Extension-метод для IEnumerable, а не для IQueryable, соответветственно, произойдет запрос всех данных таблицы в память и только потом выполнится where
		}
		public TEntity FindById(int id)
		{
			return _dbSet.Find(id);
		}

		public void Create(TEntity item)
		{
			_dbSet.Add(item);			
		}
		public void Update(TEntity item)
		{
			_context.Entry(item).State = EntityState.Modified;//DbContext и сам в состоянии это сделать если сущность изменилась
		}
		public void Remove(TEntity item)
		{
			_dbSet.Remove(item);
		}
	}
}
