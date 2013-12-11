namespace EnergyTrading.Test.Rhino
{
    public static class MockExtensions
    {
        public static CaptureExpression<T> Capture<T>(this T stub)
            where T : class
        {
            return new CaptureExpression<T>(stub);
        }
    }
}
