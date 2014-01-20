using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Bus;
using NServiceBus;

namespace CQRS.NServiceBus
{
    /// <summary>
    /// Decorator which processes INServiceBusEventMessages (using nservicebus), then digs out their Real Event and punts it onto an IEventBus
    /// </summary>
    public class NServiceBusEventMessageHandler : IHandleMessages<INServiceBusEventMessage>
    {
        private readonly IEventBus eventBus;

        public NServiceBusEventMessageHandler(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }

        public void Handle(INServiceBusEventMessage message)
        {
            var @event = message.RealEvent;
            eventBus.Publish(@event);
        }
    }
}
