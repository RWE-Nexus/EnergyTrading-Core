namespace EnergyTrading.UnitTest.Container.Unity.AutoRegistration
{
    using System;

    public class DomainEventHandlerOne : IHandlerFor<DomainEvent>
    {
        public void Handle(DomainEvent e)
        {
            throw new NotImplementedException();
        }
    }
}