using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SqlHealthMonitor.BLL.Services;
using SqlHealthMonitor.DAL.DatabaseContext;
using SqlHealthMonitor.DAL.Helpers;
using SqlHealthMonitor.DAL.Repositories;
using SqlHealthMonitor.Infrastructure;
using SqlHealthMonitor.WEB.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SqlHealthMonitor
{
        public class MvcApplication : HttpApplication, IContainerAccessor
        {
            static IWindsorContainer container;

            public IWindsorContainer Container => container;

            public void Application_End()
            {
                container.Dispose();
            }

            protected void application_start()
            {
                container = new WindsorContainer();
                container.Register(Component.For<IPrincipal>()
                    .LifeStyle.PerWebRequest
                    .UsingFactoryMethod(() => HttpContext.Current.User));
                container.Register(Component.For<ICacheService>().ImplementedBy<CacheService>().LifestyleTransient());
                container.Register(Component.For<CacheInterceptor>().LifestyleTransient());
              
                container.Register(Component.For<DbContext>().ImplementedBy<SqlHealthMonitorDbContext>().LifestylePerWebRequest());
                container.Register(Classes.FromAssemblyContaining<ServiceBase>().BasedOn<IService>()
                    .WithServiceDefaultInterfaces().LifestyleTransient().Configure(ConfigureInterceptors));
                container.Register(Classes.FromAssemblyContaining(typeof(RepositoryBase<>))
                    .BasedOn(typeof(IRepository<>)).WithServiceDefaultInterfaces().LifestyleTransient());
                container.Register(Classes.FromAssemblyContaining<IHelper>().BasedOn<IHelper>()
                    .WithServiceDefaultInterfaces().LifestyleTransient());
                container.Register(Classes.FromAssemblyContaining<HomeController>().BasedOn<IController>().LifestyleTransient());


                ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container.Kernel));
                //DefaultHubManager hd = new DefaultHubManager(GlobalHost.DependencyResolver);
                //var hub = hd.ResolveHub("TaskManagerHub") as TaskManagerHub;
                //   var hub = GlobalHost.ConnectionManager.GetHubContext<TaskManagerHub>();
                //var t = hubi.CurrentTasks.Count;
                //var hub = new TaskManagerHub();
            

            }
            private void ConfigureInterceptors(ComponentRegistration obj)
            {
                var reg = obj.Interceptors(InterceptorReference.ForType<CacheInterceptor>()).First;
            }
        }
    }

