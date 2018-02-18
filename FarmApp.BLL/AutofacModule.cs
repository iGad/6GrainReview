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
            //здесь бы я зарегистрировал в первую очередь FarmContext, обязательно InstancePerRequest, чтобы контейнер управлял временм жизни.
            //зарегистрировал все конкретные репозитории как реализации интерфейса: builder.RegisterType<GenericRepository<Farm>>().As<IRepository<Farm>>().InstancePerDependency() - нет смысла использовать InstancePerRequest, т.к. создание репозитория простая операция, а контекст у нескольких репозиториев будет один
            //IMapper можно зарегистрировать как builder.RegisterInstance(AutoMapperConfig.GetMapper()).As<IMapper>(), чтобы он загрузил конфигурацию один раз. Дополнение. Регистрировать в проекте FarmApp.Web
            //FarmService тоже сделал бы InstancePerDependency, чтобы он уничтожался сразу как пропадает ссылка на него. В совокупности с предыдущим пунктом, убрал бы WithParameter("map", AutoMapperConfig.GetMapper())
            builder.RegisterType<EFUnitOfWork>().As<IUnitOfWork>().WithParameter("connectionString", "FarmContext").InstancePerRequest();
			builder.RegisterType<FarmService>().As<IFarmService>().WithParameter("map", AutoMapperConfig.GetMapper()).InstancePerRequest();
		    
			base.Load(builder);
		}
	}
}
