﻿using FarmApp.DAL.EF;
using FarmApp.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmApp.DAL.Repositories
{
    //Смущает реализация IDisposable. По-моему не хватает деструктора, который вызывает Dispose(false), хотя, вообще говоря, когда нет неуправляемых ресурсов, смысла в IDisposable мало
    //Приватные поля лучше именовать по-особому или обращаться к ним строго через this, чтобы не было путаницы с локальными переменными
    /// <summary>
    /// Реализация паттерна uow
    /// </summary>
	public class EFUnitOfWork : IUnitOfWork
	{
		private FarmContext context;

		private IRepository<Agriculture> agricultures;

		private IRepository<Crop> crops;

		private IRepository<Farm> farms;

		private IRepository<Farmer> farmers;

		private IRepository<Region> regions;

		public EFUnitOfWork(string connectionString)
		{
			context = new FarmContext(connectionString);
		}

		public EFUnitOfWork()
		{
			context = new FarmContext();
		}
        //можно воспользоваться ?? и фишками с# 6.0 и записать эти 10 строк как
        //public IRepository<Agriculture> Agricultures => agricultures ?? (agricultures = new EFRepository<Agriculture>(context));
        public IRepository<Agriculture> Agricultures
		{
			get
			{
				if (agricultures == null)
				{
					agricultures = new EFRepository<Agriculture>(context);
				}
				return agricultures;
			}
		}

		public IRepository<Crop> Crops
		{
			get
			{
				if (crops == null)
				{
					crops = new EFRepository<Crop>(context);
				}
				return crops;
			}
		}

		public IRepository<Farm> Farms
		{
			get
			{
				if (farms == null)
				{
					farms = new EFRepository<Farm>(context);
				}
				return farms;
			}
		}

		public IRepository<Farmer> Farmers
		{
			get
			{
				if (farmers == null)
				{
					farmers = new EFRepository<Farmer>(context);
				}
				return farmers;
			}
		}

		public IRepository<Region> Regions
		{
			get
			{
				if (regions == null)
				{
					regions = new EFRepository<Region>(context);
				}
				return regions;
			}
		}
		
		public void Save()
		{
			context.SaveChanges();
		}

		private bool disposed = false;

		public virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					context.Dispose();
				}
				this.disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
