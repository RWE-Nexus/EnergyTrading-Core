namespace EnergyTrading.Mapping
{
    using System;

    /// <summary>
    /// Type of the mapping between an object and XML.
    /// </summary>
    [Flags]
    public enum XmlMapTarget
    {
        Value = 0x1,
        Id = 0x2,
        Entity = 0x4,
        Collection  = 0x8,
        Count = 0x16
    }
}