using Autofac;

namespace ABO.Core.Infrastructure
{
    public interface IDependencyRegistrar
    {
        void Register(ContainerBuilder builder);
        int Order { get; }
    }
}
