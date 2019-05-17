using ABO.Core.Data;
using ABO.Data;
using ABO.Services.Security;
using Autofac;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Tests.Services
{
    [TestFixture]
    public class PermissionServiceTests : TestSuiteBase
    {
        [SetUp]
        public void SetUp()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<SqlServerDataProvider>().As<IDataProvider>().SingleInstance();
            builder.RegisterType<ABODataContext>().As<IDbContext>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            builder.Update(ContainerManager.Container);
        }

        [Test]
        [Category(TestCategory.IntegrationTest)]
        public void AuthorizeUser()
        {
            var userId = 1; // VNM00000 user

            var permissionService = ContainerManager.ResolveUnregistered<PermissionService>();
            var authorized = permissionService.Authorize(userId, Core.UserPermission.ManageDistributorProfile);

            Assert.IsTrue(authorized);
        }
    }
}
