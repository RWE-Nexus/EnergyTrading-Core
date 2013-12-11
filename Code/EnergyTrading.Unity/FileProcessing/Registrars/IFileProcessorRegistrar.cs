using Microsoft.Practices.Unity;

namespace EnergyTrading.FileProcessing.Registrars
{
    public interface IFileProcessorRegistrar
    {
        void Register(IUnityContainer container, FileProcessorEndpoint fileProcessorEndpoint);
    }
}