using FarmApp.DAL.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FarmApp.DAL.Tests
{
    //Странная идея в тестировании EFUnitOfWork, в котором ничего не делается. 
    //это уже не юнит-тесты. Юнит-тесты не должны работать с файлами и с БД. Это тогда интеграционные тесты, надо их хоть атрибутом что ли пометить, чтобы отличать.
    //Если уж тестировать EFUnitOfWork, то надо создать мок для FarmContext, который не работает с БД и с файлом
    //Наименование тестов не позволяет понять что тестируется. Лично я за использование наименования Given..._When...._Then... 
    //В данном случае, т.к. EFUnitOfWork почти не содержит логику, тесты тестируют непонятно что
    //Я за подход AAA (Arrange-Act-Assert), каждый из которых отделять пустой строкой, а сложную логику подготовки выносить в методы, чтобы всегда было понятно где в теле теста что происходит
    [TestFixture]
	public class EFUnitOfWorkTests
	{
		[SetUp]
		public void Configure()
		{
			AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "../../../FarmApp/App_Data"));
		}

        //без комментариев
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

	    //без комментариев
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
        //в тех методах хоть Assert есть, а тут даже его нет.
        //данные тесты не пройдут если их запускать параллельно
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
