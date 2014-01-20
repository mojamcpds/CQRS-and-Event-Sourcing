using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace CQRS.EventStore
{
    public interface IEventStore
    {
        void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion);
        List<Event> GetEventsForAggregate(Guid aggregateId);
    }
}
