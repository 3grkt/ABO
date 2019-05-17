using ABO.Core;
using ABO.Core.Data;
using ABO.Core.Infrastructure;
using ABO.Data;
using ABO.Services.Caching;
using ABO.Services.DataPurges;
using ABO.Services.Distributors;
using ABO.Services.ImportExport;
using ABO.Services.Localization;
using ABO.Services.Logging;
using ABO.Services.Profiles;
using ABO.Services.Security;
using ABO.Services.Users;
using ABO.Services.WareHouse;
using ABO.Web.Framework;
using ABO.Web.Helpers;
using Autofac;
using Autofac.Integration.Mvc;
using System.Reflection;
using ABO.Core.Domain;
using ABO.Core.Domain.WTA;
using Autofac.Core;

namespace ABO.Web.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder containerBuilder)
        {
            // Controllers
            containerBuilder.RegisterControllers(Assembly.GetExecutingAssembly());

            // DbContext
            containerBuilder.RegisterType<ABODataContext>().As<IDbContext>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DSSDataContext>().Named<IDbContext>("DSSDataContext").InstancePerLifetimeScope();
            containerBuilder.RegisterType<WTADataContext>().Named<IDbContext>("WTADataContext").InstancePerLifetimeScope();

            // Repository
            // containerBuilder.RegisterType<EfRepository<User>>().As<IRepository<User>>().InstancePerHttpRequest();
            containerBuilder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            containerBuilder.RegisterType<EfRepository<ProspectAvatar>>()
                .As<IRepository<ProspectAvatar>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("WTADataContext"));

            // Data Provider
            containerBuilder.RegisterType<SqlServerDataProvider>().As<IDataProvider>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DataHelper>().As<IDataHelper>().InstancePerLifetimeScope();

            // Services
            containerBuilder.RegisterType<WebResourceManager>().As<IResourceManager>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<FileLogger>().As<ILogger>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<PermissionService>().As<IPermissionService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ProfileTypeService>().As<IProfileTypeService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ProfileService>().As<IProfileService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DistributorService>().As<IDistributorService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<WareHouseService>().As<IWarehouseService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ProfileBoxService>().As<IProfileBoxService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<DataPurgeService>().As<IDataPurgeService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ExcelParamManager>().As<IExcelParamManager>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ExportManager>().As<IExportManager>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<CacheService>().As<ICacheService>().InstancePerLifetimeScope();
            containerBuilder.RegisterType<ProfileImageMergingService>()
                .As<IProfileImageMergingService>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>("WTADataContext"))
                .InstancePerLifetimeScope();

            // WebHelper
            containerBuilder.RegisterType<WebHelper>().As<IWebHelper>().SingleInstance();

            // WorkContext
            containerBuilder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerLifetimeScope();
        }

        public int Order { get { return 0; } }
    }
}