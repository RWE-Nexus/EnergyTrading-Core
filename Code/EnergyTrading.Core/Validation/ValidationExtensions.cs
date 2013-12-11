namespace EnergyTrading.Validation
{
    using System.Collections.Generic;

    public static class ValidationExtensions
    {
        public static void Validate<T>(this IValidator<T> validator, T item)
        {
            var errors = new List<IRule>();
            if (!validator.IsValid(item, errors))
            {
                throw new ValidationException(errors);
            }
        }
    }
}