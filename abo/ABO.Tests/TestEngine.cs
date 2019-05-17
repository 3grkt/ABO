using ABO.Core.Configuration;
using ABO.Core.Data;
using ABO.Core.Infrastructure;
using ABO.Data;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Tests
{
    public class TestEngine : IEngine
    {
        private ContainerManager _containerManager;

        #region Utilities
        private void RegisterDependencies(ABOConfig config, params IDependencyRegistrar[] dependencyRegistars)
        {
            var builder = new ContainerBuilder();
            var container = builder.Build();

            builder = new ContainerBuilder();
            builder.RegisterType<ABODataContext>().Named<IDbContext>("SqlServerContext").SingleInstance();

            // Repository
            builder.RegisterGeneric(typeof(EfRepository<>)).Named("SqlServerRepository", typeof(IRepository<>))
                .WithParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IDbContext),
                    (pi, ctx) => ctx.ResolveKeyed<IDbContext>("SqlServerContext"))
                .InstancePerLifetimeScope();

            // Service
            //builder.RegisterType<AssetService>().Named<IAssetService>("SqlServerService").InstancePerLifetimeScope();

            builder.Update(container);

            // Set container manager
            _containerManager = new ContainerManager(container, new TestContextManager());
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
        #endregion

        #region IEngine Members

        public ContainerManager ContainerManager
        {
            get { return _containerManager; }
        }

        public void Initialize(ABOConfig config, params IDependencyRegistrar[] dependencyRegistars)
        {
            RegisterDependencies(config, dependencyRegistars);
        }

        public T Resolve<T>(string key = null) where T : class
        {
            if (!string.IsNullOrEmpty(key))
                return ContainerManager.Resolve<T>(key);

            return ContainerManager.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return ContainerManager.Resolve(type);
        }

        public T[] ResolveAll<T>()
        {
            return ContainerManager.ResolveAll<T>();
        }

        #endregion
    }
}
