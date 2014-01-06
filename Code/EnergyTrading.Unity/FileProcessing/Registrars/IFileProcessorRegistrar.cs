using Microsoft.Practices.Unity;

namespace EnergyTrading.FileProcessing.Registrars
{
    using EnergyTrading.FileProcessing;

    public interface IFileProcessorRegistrar
    {
        void Register(IUnityContainer container, FileProcessorEndpoint fileProcessorEndpoint);
    }
}