using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CQRS.NServiceBus
{
    public class DomainEventBusConfig : ConfigurationSection
    {
        private const string DomainEventEndpointMappingsElementName = "DomainEventEndpointMappings";

        [ConfigurationProperty(DomainEventEndpointMappingsElementName, IsRequired = false)]
        public DomainEventEndpointMappingCollection DomainEventEndpointMappings
        {
            get { return (base[DomainEventEndpointMappingsElementName] as DomainEventEndpointMappingCollection); }
            set { base[DomainEventEndpointMappingsElementName] = value; }
        }
    }
}
