using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace CQRS.Bus
{
    public interface IRouteMessages
    {
        void Route<T>(T message) where T : Message;
        void RegisterHandler<T>(Action<T> handler) where T : Message;
    }
}
