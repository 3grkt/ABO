using Autofac;

namespace ABO.Core.Infrastructure
{
    public interface IContextManager
    {
        ILifetimeScope GetContextLifetimeScope();
    }
}
