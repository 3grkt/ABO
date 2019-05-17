using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;

namespace ABO.Core.Infrastructure
{
    public class CoreDependencyRegistrar : IDependencyRegistrar
    {
        #region IDependencyRegistrar Members

        public void Register(Autofac.ContainerBuilder builder)
        {
            builder.RegisterType<AppSettings>().As<IAppSettings>().SingleInstance();
        }

        public int Order
        {
            get { return 0; }
        }

        #endregion
    }
}
