namespace EnergyTrading.Mapping
{
    /// <summary>
    /// Provides a downcast from an IXmlMapper{S, D} when we want to be polymorphic on D
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IXmlMapper<in TSource>
    {
        object Map(TSource source, string nodeName, string xmlNamespace = null, string xmlPrefix = "", int index = -1);
    }
}