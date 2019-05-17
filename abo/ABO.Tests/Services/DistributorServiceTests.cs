using ABO.Core.Data;
using ABO.Data;
using ABO.Services.Distributors;
using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Tests.Services
{
    public class DistributorServiceTests : TestSuiteBase
    {
        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SqlServerDataProvider>().As<IDataProvider>().SingleInstance();
            builder.RegisterType<ABODataContext>().As<IDbContext>().InstancePerLifetimeScope();
            builder.RegisterType<DSSDataContext>().Named<IDbContext>("DSSDataContext").InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.Update(ContainerManager.Container);
        }

        [Test]
        [Category(TestCategory.IntegrationTest)]
        public void GetDSSDistributors()
        {
            var testDistNumbers = new long[] { 2008941, 2009076, 2009093, 2009527, 2009663, 2010311 };
            var distributorService = ContainerManager.ResolveUnregistered<DistributorService>();

            var foundDists = distributorService.GetDSSDistributors(testDistNumbers);
            Assert.GreaterOrEqual(foundDists.Count, 0);

            PrintDSSDistributors(foundDists);
        }
    }
}
