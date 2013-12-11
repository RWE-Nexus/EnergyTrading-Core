namespace EnergyTrading.FileProcessing.Configuration
{
    using EnergyTrading.Configuration;

    public class FileProcessorCollection : NamedConfigElementCollection<FileProcessorElement>
    {
        protected override string ElementName
        {
            get { return "processor"; }
        }
    }
}