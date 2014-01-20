using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace CQRS.Bus
{
    public interface IEventBus
    {
        void Publish<T>(T @event) where T : Event;
    }
}
