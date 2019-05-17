using ABO.Core;
using ABO.Core.Data;
using ABO.Core.Infrastructure;
using ABO.Data;
using ABO.Jobs.Workers;
using ABO.Services.Caching;
using ABO.Services.DataPurges;
using ABO.Services.Distributors;
using ABO.Services.Localization;
using ABO.Services.Logging;
using ABO.Services.Profiles;
using ABO.Services.Security;
using ABO.Services.Users;
//using ABO.Web.Framework;
using Autofac;
//using Autofac.Integration.Mvc;
using System.Reflection;

namespace ABO.Jobs.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            //// Controllers
            //containerBuilder.RegisterControllers(Assembly.GetExecutingAssembly());

            // DbContext
            containerBuilder.RegisterType<ABODataContext>().As<IDbContext>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DSSDataContext>().Named<IDbContext>("DSSDataContext").InstancePerLifetimeScope();
            //containerBuilder.RegisterType<WTADataContext>()
            //    .Named<IDbContext>("WTADataContext")
            //    .InstancePerLifetimeScope();

            // Repository
            //containerBuilder.RegisterType<EfRepository<User>>().As<IRepository<User>>().InstancePerHttpRequest();
            containerBuilder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();

            // Data Provider
            containerBuilder.RegisterType<SqlServerDataProvider>().As<IDataProvider>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DataHelper>().As<IDataHelper>().InstancePerLifetimeScope();

            // Services
            //containerBuilder.RegisterType<WebResourceManager>().As<IResourceManager>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FileLogger>().As<ILogger>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ProfileService>().As<IProfileService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DistributorService>().As<IDistributorService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DataPurgeService>().As<IDataPurgeService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<CacheService>().As<ICacheService>().InstancePerLifetimeScope();

            // Workers
            //containerBuilder.Register(c => new ProfileScanWorker(c.Resolve<IProfileService>())).InstancePerLifetimeScope();
            containerBuilder.RegisterType<ProfileScanWorker>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DataPurgeWorker>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DistributorUpdateWorker>().InstancePerLifetimeScope();

            // WorkContext
            //containerBuilder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
        }

        public int Order { get { return 0; } }
    }
}