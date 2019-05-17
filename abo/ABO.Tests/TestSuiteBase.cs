using ABO.Core.Configuration;
using ABO.Core.Data;
using ABO.Core.Infrastructure;
using ABO.Data;
using ABO.Services.Localization;
using DSS = ABO.Core.Domain.DSS;
using Autofac;
using System;
using System.Collections.Generic;

namespace ABO.Tests
{
    public abstract class TestSuiteBase
    {
        //private object AssetService;
        public static ContainerManager ContainerManager { get { return EngineContext.Current.ContainerManager; } }

        static TestSuiteBase()
        {
            //RegisterDependencies();
            EngineContext.CreateEngineInstance = () => new TestEngine();
            EngineContext.Initialize();
        }


        //private static void RegisterDependencies()
        //{
        //    var builder = new ContainerBuilder();
        //    var container = builder.Build();

        //    builder = new ContainerBuilder();
        //    //builder.RegisterType<AMSDataContext>().As<IDbContext>().InstancePerLifetimeScope();
        //    //builder.RegisterType<AMSDataContext>().Named<IDbContext>("SqlServerContext").InstancePerLifetimeScope();
        //    builder.RegisterType<AMSDataContext>().Named<IDbContext>("SqlServerContext").SingleInstance();

        //    // Repository
        //    builder.RegisterGeneric(typeof(EfRepository<>)).Named("SqlServerRepository", typeof(IRepository<>))
        //        .WithParameter(
        //            (pi, ctx) => pi.ParameterType == typeof(IDbContext),
        //            (pi, ctx) => ctx.ResolveKeyed<IDbContext>("SqlServerContext"))
        //        //.WithParameter(new TypedParameter(typeof(IDbContext), new AMSDataContext()))
        //        .InstancePerLifetimeScope();

        //    // Service
        //    builder.RegisterType<AssetService>().Named<IAssetService>("SqlServerService").InstancePerLifetimeScope();

        //    builder.Update(container);

        //    ContainerManager = new ContainerManager(container, new TestContextManager());
        //}

        #region Methods

        protected void PrintDSSDistributors(IList<DSS.Distributor> distributors)
        {
            foreach (var dist in distributors)
            {
                Console.WriteLine(
                    dist.DistributorNo + "\t" +
                    dist.Name1 + "\t" +
                    dist.Sex1 + "\t" +
                    dist.City + "\t" +
                    dist.Mobile + "\t" +
                    dist.Award + "\t");
            }
        }

        #endregion
    }
}
