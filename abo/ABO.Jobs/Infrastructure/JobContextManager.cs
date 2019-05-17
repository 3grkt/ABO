using ABO.Core.Infrastructure;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ABO.Jobs.Infrastructure
{
    public class JobContextManager : IContextManager
    {
        private IContainer _currentContainer;
        private ILifetimeScope _currentScope;

        public JobContextManager(IContainer container)
        {
            _currentContainer = container;
        }

        protected ILifetimeScope CurrentScope
        {
            get
            {
                if (_currentScope == null)
                    _currentScope = _currentContainer.BeginLifetimeScope("JobContextManager");
                return _currentScope;
            }
        }

        #region IContextManager Members

        public Autofac.ILifetimeScope GetContextLifetimeScope()
        {
            return CurrentScope;
        }

        #endregion
    }
}
