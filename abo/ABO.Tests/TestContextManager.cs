using ABO.Core.Infrastructure;
using Autofac.Core;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Tests
{
    public class TestContextManager : IContextManager
    {
        #region IContextManager Members

        public Autofac.ILifetimeScope GetContextLifetimeScope()
        {
            return null;
        }

        #endregion
    }
}
