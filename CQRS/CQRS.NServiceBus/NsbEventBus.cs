using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Bus;
using CQRS.Core;
using NServiceBus;

namespace CQRS.NServiceBus
{
    public class NsbEventBus : IEventBus
    {
        private readonly IBus _bus;

        public NsbEventBus(IBus bus)
        {
            _bus = bus;
        }


        private static INServiceBusEventMessage CreateDomainEventMessage(Event @event)
        {
            var domainEventMessageType = typeof(NServiceBusEventMessage<>).MakeGenericType(@event.GetType());
            var message = (INServiceBusEventMessage)Activator.CreateInstance(domainEventMessageType);
            message.RealEvent = @event;
            return message;
        }

        public void Publish<T>(T @event) where T : Event
        {
            _bus.Publish(CreateDomainEventMessage(@event));
        }
    }
}
