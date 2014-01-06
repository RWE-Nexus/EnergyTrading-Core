namespace EnergyTrading.Registrars
{
    using EnergyTrading.Wrappers;

    using Microsoft.Practices.Unity;

    using EnergyTrading.Container.Unity;

    /// <summary>
    /// Registers standard framework implementations for <see cref="Wrappers" /> which allow abstraction over standard .NET capabilities e.g. file system, DateTime.Now etc
    /// </summary>
    public class WrappersRegistrar : IContainerRegistrar
    {
        /// <copydocfrom cref="IContainerRegistrar.Register" />
        public void Register(IUnityContainer container)
        {
            container.RegisterType<IFile, FileWrapper>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDirectory, DirectoryWrapper>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDateTime, DateTimeWrapper>(new ContainerControlledLifetimeManager());
        }
    }
}