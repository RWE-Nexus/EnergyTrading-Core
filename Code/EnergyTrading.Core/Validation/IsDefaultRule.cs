namespace EnergyTrading.Validation
{
    public class IsDefaultRule<T> : Rule<T>
    {
        public override bool IsValid(T entity)
        {
            return Equals(entity, default(T));
        }
    }
}