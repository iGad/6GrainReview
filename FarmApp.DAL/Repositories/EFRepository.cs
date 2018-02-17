using FarmApp.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FarmApp.DAL.Repositories
{
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
			return _dbSet.Where(predicate);
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
			_context.Entry(item).State = EntityState.Modified;
		}
		public void Remove(TEntity item)
		{
			_dbSet.Remove(item);
		}
	}
}
