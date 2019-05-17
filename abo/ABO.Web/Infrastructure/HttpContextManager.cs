using ABO.Core.Infrastructure;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ABO.Web.Infrastructure
{
    public class HttpContextManager : IContextManager
    {
        #region IContextManager Members

        public Autofac.ILifetimeScope GetContextLifetimeScope()
        {
            if (HttpContext.Current != null)
                return AutofacDependencyResolver.Current.RequestLifetimeScope;
            return null;
        }

        #endregion
    }
}