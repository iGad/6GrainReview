namespace FarmApp.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FarmApp.DAL.EF.FarmContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
			ContextKey = "DAL.MigratoryFarmsContext";
		}

        protected override void Seed(FarmApp.DAL.EF.FarmContext context)
        {
			//  This method will be called after migrating to the latest version.

			//  You can use the DbSet<T>.AddOrUpdate() helper extension method 
			//  to avoid creating duplicate seed data.
			
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var msk = new Region() { Name = "Сочи" };
                    var psb = new Region() { Name = "Краснодар" };

                    context.Regions.AddOrUpdate(x => x.Name, msk);
                    context.Regions.AddOrUpdate(x => x.Name, psb);

                    var rye = new Agriculture() { Name = "Рожь" };
                    var wheat = new Agriculture() { Name = "Пшеница" };

                    context.Agricultures.AddOrUpdate(x => x.Name, rye);
                    context.Agricultures.AddOrUpdate(x => x.Name, wheat);

                    var vasya = new Farmer() { Name = "Василий Петрович" };

                    var parfen = new Farmer() { Name = "Парфён Семёнович" };

                    context.Farmers.AddOrUpdate(x => x.Name, vasya);
                    context.Farmers.AddOrUpdate(x => x.Name, parfen);

                    context.SaveChanges();

                    var smallFarm = new Farm() { Name = "Сочинская ферма Василия Петровича", Area = 100, FarmerId = vasya.Id, RegionId = msk.Id };

                    var bigFarm = new Farm() { Name = "ЗАО Краснодарский совхоз Парфёна", Area = 600, FarmerId = parfen.Id, RegionId = psb.Id };

                    context.Farms.AddOrUpdate(x => x.Name, smallFarm);
                    context.Farms.AddOrUpdate(x => x.Name, bigFarm);

                    context.SaveChanges();

                    var cropRyeSmallFarm = new Crop() { AgricultureId = rye.Id, FarmId = smallFarm.Id, Gather = 250 };
                    var cropWheatSmallFarm = new Crop() { AgricultureId = wheat.Id, FarmId = smallFarm.Id, Gather = 139 };
                    var cropRyeBigFarm = new Crop() { AgricultureId = rye.Id, FarmId = bigFarm.Id, Gather = 1100 };
                    var cropWheatBigFarm = new Crop() { AgricultureId = wheat.Id, FarmId = bigFarm.Id, Gather = 900 };

                    context.Crops.AddOrUpdate(x=> new { x.AgricultureId, x.FarmId },  cropRyeSmallFarm);
                    context.Crops.AddOrUpdate(x => new { x.AgricultureId, x.FarmId }, cropWheatSmallFarm);
                    context.Crops.AddOrUpdate(x => new { x.AgricultureId, x.FarmId }, cropRyeBigFarm);
                    context.Crops.AddOrUpdate(x => new { x.AgricultureId, x.FarmId }, cropWheatBigFarm);


                    context.SaveChanges();

                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                }

            }
		}
    }
}
