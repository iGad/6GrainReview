using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;
using FarmApp.BLL;
using System.Web.Mvc;

namespace FarmApp.Util
{
	public class AutofacConfig
	{
		public static void ConfigureContainer()
		{
			// получаем экземпляр контейнера
			var builder = new ContainerBuilder();

			//регистрируем модуль из проекта BLL
			builder.RegisterModule(new AutofacModule());

			// регистрируем контроллер в текущей сборке
			builder.RegisterControllers(typeof(MvcApplication).Assembly).WithParameter(
				(p, c) => p.ParameterType == typeof(IMapper), 
				(p, c) => AutoMapperConfig.GetMapper()
            );

			// создаем новый контейнер с теми зависимостями, которые определены выше
			var container = builder.Build();

			// установка сопоставителя зависимостей
			DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
		}
	}
}