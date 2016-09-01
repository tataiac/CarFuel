using Autofac;
using Autofac.Integration.Mvc;
using CarFuel.DataAccess;
using CarFuel.DataAccess.Bases;
using CarFuel.DataAccess.Contexts;
using CarFuel.Models;
using CarFuel.Services;
using CarFuel.Services.Bases;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CarFuel.Web {
  public class MvcApplication : HttpApplication {
    protected void Application_Start() {
      initAutoFac();

      AreaRegistration.RegisterAllAreas();
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
    }

    private void initAutoFac() {
      var builder = new ContainerBuilder();

      builder.RegisterControllers(typeof(MvcApplication).Assembly);

      //Repository
      builder.RegisterType<CarRepository>().AsSelf().As<IRepository<Car>>();
      builder.RegisterType<UserRepository>().AsSelf().As<IRepository<User>>();

      //Service
      //builder.RegisterType<CarServices>().AsSelf().As<IService<Car>>();
      builder.RegisterType<CarServices>().AsSelf().As<IService<Car>>().As<ICarService>();
      //builder.RegisterType<UserServices>().AsSelf().As<IService<User>>();
      builder.RegisterType<UserServices>().AsSelf().As<IService<User>>().As<IUserService>().InstancePerLifetimeScope();

      //DbContext
      builder.RegisterType<CarFuelDb>().As<DbContext>().InstancePerLifetimeScope();//For sharing the same object db.

      var container = builder.Build();
      DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
    }
  }
}
