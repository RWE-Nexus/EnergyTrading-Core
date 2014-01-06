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
            get { return this.propertyCheck; }
        }

        public PropertyCheckExpression Id()
        {
            this.propertyCheck.CompareTarget = CompareTarget.Id;

            return this;
        }

        public PropertyCheckExpression Entity()
        {
            this.propertyCheck.CompareTarget = CompareTarget.Entity;

            return this;
        }

        public PropertyCheckExpression Count()
        {
            this.propertyCheck.CompareTarget = CompareTarget.Count;

            return this;
        }

        public PropertyCheckExpression Value()
        {
            this.propertyCheck.CompareTarget = CompareTarget.Value;

            return this;
        }

        public PropertyCheckExpression Length(int value)
        {
            this.propertyCheck.Length = value;

            return this;
        }
    }
}