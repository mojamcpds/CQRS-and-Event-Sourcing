using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using CQRS.Core;

namespace CQRS.EventStore
{
    public class EventDescriptor
    {
        private readonly IFormatter formatter = new BinaryFormatter();

        public Event Event { get; private set; }
        public Guid AggregateId { get; private set; }
        public int Version { get; private set; }

        public EventDescriptor(Guid aggregateId, Event @event, int version)
        {
            Event = @event;
            Version = version;
            AggregateId = aggregateId;
        }

    }
}
