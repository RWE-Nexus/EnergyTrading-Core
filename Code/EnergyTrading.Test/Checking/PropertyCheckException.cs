namespace EnergyTrading.Test.Checking
{
    using System;

    public class PropertyCheckException : Exception
    {
        private const string Format = "{0}: Expected:<{1}>. Actual:<{2}>";

        public PropertyCheckException(string propertyName, object expected, object actual)
            : base(string.Format(Format, propertyName, expected, actual))
        {
            this.PropertyName = propertyName;
            this.ExpectedValue = expected;
            this.ActualValue = actual;
        }

        public string PropertyName { get; private set; }

        public object ExpectedValue { get; private set; }

        public object ActualValue { get; private set; }
    }
}