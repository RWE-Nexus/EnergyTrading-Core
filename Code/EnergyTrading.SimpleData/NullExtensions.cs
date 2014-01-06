namespace EnergyTrading.Data.SimpleData
{
    using System;

    public static class NullExtensions
    {
        public static bool IsDbNull(this object value)
        {
            return value == DBNull.Value;
        }
        public static T DefaultIfDbNull<T>(this object value, T defaultValue)
        {
            return value.IsDbNull() ? defaultValue : (T)value;
        }
    }
}