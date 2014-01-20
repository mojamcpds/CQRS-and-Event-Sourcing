using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Bus;
using CQRS.Configuration;
using Ninject;
using NServiceBus;
using NServiceBus.Unicast;

namespace CQRS.NServiceBus
{
    public class ConfigEventBus : Configure
    {
        private readonly Configure configure;
        private readonly IKernel _kernel;

        public ConfigEventBus(Configure configure, IKernel kernel)
        {
            this.configure = configure;
            _kernel = kernel;
        }

        public ConfigEventBus SubscribeForDomainEvents()
        {
            BusBootStrapper.BootStrap(_kernel);
            var eventBus = _kernel.Get<IEventBus>();

            // Override the IEventBus in IoC with the NsB one
            configure.Configurer.RegisterSingleton<IEventBus>(eventBus);

            //var configEventBus = new ConfigEventBus();
            //configEventBus.Configure(this);

            this.Configure(configure);

            return this;
        }


        public void Configure(Configure configure1)
        {
            var domainEventBusConfig = GetConfigSection<DomainEventBusConfig>();

            var domainEventTypes = HandlerHelper.GetEvents();

            var domainEventsTypesWrappedWithNServiceBusType = new List<Type>();

            var bus = (UnicastBus)configure1
                .MsmqTransport()
                .UnicastBus()
                    .LoadMessageHandlers(new First<NServiceBusEventMessageHandler>())
                    .CreateBus();

            RegisterAssemblyDomainEventSubscriptionMappings(domainEventBusConfig, domainEventTypes, domainEventsTypesWrappedWithNServiceBusType, bus);

            // TODO: Cast to UnicastBus isn't working
            bus.Started += (s, e) => domainEventsTypesWrappedWithNServiceBusType.ForEach(bus.Subscribe);
        }

        private static void RegisterAssemblyDomainEventSubscriptionMappings(DomainEventBusConfig domainEventBusConfig, IEnumerable<Type> domainEventTypes, ICollection<Type> domainEventMessageTypes, UnicastBus bus)
        {
            var domainEventMessageType = typeof(NServiceBusEventMessage<>);
            foreach (DomainEventEndpointMapping mapping in domainEventBusConfig.DomainEventEndpointMappings)
            {
                foreach (var domainEventType in domainEventTypes)
                {
                    if (DomainEventsIsAssembly(domainEventType, mapping.DomainEvents))
                    {
                        var messageType = domainEventMessageType.MakeGenericType(domainEventType);
                        domainEventMessageTypes.Add(messageType);
                        bus.RegisterMessageType(messageType, mapping.Endpoint, false);
                    }
                }
            }
        }

        private static bool DomainEventsIsDomainEvent(Type domainEventType, string domainEvents)
        {
            return domainEventType.FullName.ToLower() == domainEvents.ToLower()
                   || domainEventType.AssemblyQualifiedName.ToLower() == domainEvents.ToLower();
        }

        private static bool DomainEventsIsAssembly(Type domainEventType, string domainEvents)
        {
            return domainEventType.Assembly.GetName().Name.ToLower() == domainEvents.ToLower();
        }
    }
}
