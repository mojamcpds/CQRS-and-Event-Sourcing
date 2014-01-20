using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace CQRS.EventStore
{
    public class DomainRepository<T> : IDomainRepository<T> where T : AggregateRoot, new() 
    {
        private readonly IEventStore _storage;

        public DomainRepository(IEventStore storage)
        {
            _storage = storage;
        }

        public void Save(T aggregate, int expectedVersion)
        {
            _storage.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
        }

        public T GetById(Guid id)
        {
            var obj = new T();//lots of ways to do this
            var e = _storage.GetEventsForAggregate(id);
            obj.LoadsFromHistory(e);
            return obj;
        }
    }
}
