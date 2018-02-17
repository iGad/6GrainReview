using Autofac;
using FarmApp.BLL.Interfaces;
using FarmApp.BLL.Services;
using FarmApp.DAL.Interfaces;
using FarmApp.DAL.Repositories;

namespace FarmApp.BLL
{
	public class AutofacModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().WithParameter("connectionString", "FarmContext").InstancePerRequest();
			builder.RegisterType<FarmService>().As<IFarmService>().WithParameter("map", AutoMapperConfig.GetMapper()).InstancePerRequest();

			base.Load(builder);
		}
	}
}
