namespace EnergyTrading.UnitTest.Container.Unity.AutoRegistration
{
    public interface IHandlerFor<TEvent>
    {
        void Handle(TEvent e);
    }
}
