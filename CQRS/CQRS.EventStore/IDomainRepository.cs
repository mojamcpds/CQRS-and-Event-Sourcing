using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace CQRS.EventStore
{
    public interface IDomainRepository<T> where T : AggregateRoot, new()
    {
        void Save(T aggregate, int expectedVersion);
        T GetById(Guid id);
    }
}
