namespace EnergyTrading.Test.Checking
{
    /// <summary>
    /// Fluent interface for <see cref="PropertyCheck" />
    /// </summary>
    public class PropertyCheckExpression
    {
        private readonly PropertyCheck propertyCheck;

        /// <summary>
        /// Create a new instance of the <see cref="PropertyCheckExpression" /> class.
        /// </summary>
        /// <param name="propertyCheck"></param>
        public PropertyCheckExpression(PropertyCheck propertyCheck)
        {
            this.propertyCheck = propertyCheck;
        }

        public PropertyCheck PropertyCheck
        {
            get { return propertyCheck; }
        }

        public PropertyCheckExpression Id()
        {
            propertyCheck.CompareTarget = CompareTarget.Id;

            return this;
        }

        public PropertyCheckExpression Entity()
        {
            propertyCheck.CompareTarget = CompareTarget.Entity;

            return this;
        }

        public PropertyCheckExpression Count()
        {
            propertyCheck.CompareTarget = CompareTarget.Count;

            return this;
        }

        public PropertyCheckExpression Value()
        {
            propertyCheck.CompareTarget = CompareTarget.Value;

            return this;
        }

        public PropertyCheckExpression Length(int value)
        {
            propertyCheck.Length = value;

            return this;
        }
    }
}