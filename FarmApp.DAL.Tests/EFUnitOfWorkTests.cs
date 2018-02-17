using FarmApp.DAL.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FarmApp.DAL.Tests
{
	[TestFixture]
	public class EFUnitOfWorkTests
	{
		[SetUp]
		public void Configure()
		{
			AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../../FarmApp/App_Data"));
		}


		[Test]
		public void CanReadFarms()
		{
			IEnumerable<Farm> farms;
			using (var db = new EFUnitOfWork())
			{
				farms = db.Farms.Get().AsQueryable().ToList();
            }
			Assert.IsTrue(farms.Any());
		}

        [Test]
        public void CanAddFarmer()
        {
            var farmer = new Farmer()
            {
                Name = "Новый хитрый фермер"
            };
            using (var db = new EFUnitOfWork())
            {
                
                db.Farmers.Create(farmer);
                db.Save();
            }
            Assert.IsTrue(farmer.Id > 0);
        }

        [Test]
        public void CanDeleteCrop()
        {            
            using (var db = new EFUnitOfWork())
            {
                var crop = db.Crops.Get().FirstOrDefault();
                if(crop!=null)
                {
                    db.Crops.Remove(crop);
                }
                else
                {
                    throw new Exception("Нет ни одного объекта crop");
                }
                db.Save();
            }
        }
	}
}
