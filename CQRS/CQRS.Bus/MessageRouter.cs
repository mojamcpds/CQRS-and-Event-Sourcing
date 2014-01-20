using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using CQRS.Core;
using CQRS.Utils;

namespace CQRS.Bus
{
    public class MessageRouter : IRouteMessages
    {
        private readonly Dictionary<Type, List<Action<Message>>> _routes = new Dictionary<Type, List<Action<Message>>>();

        public void Route<T>(T message) where T : Message
        {
            if (message is Command)
            {
                RouteCommand(message as Command);
            }
            else
            {
                RouteEvent(message as Event);
            }

        }

        private void RouteEvent<T>(T @event) where T : Event
        {
            List<Action<Message>> handlers;
            _routes.TryGetValue(@event.GetType(), out handlers);

            foreach (var handler in handlers)
            {
                //dispatch on thread pool for added awesomeness
                var handler1 = handler;
                ThreadPool.QueueUserWorkItem(x => handler1(@event));
            }
        }

        private void RouteCommand<T>(T command) where T : Command
        {
            List<Action<Message>> handlers;
            _routes.TryGetValue(command.GetType(), out handlers);

            if (handlers == null)
                throw new InvalidOperationException("no handler registered");

            if (handlers.Count != 1)
                throw new InvalidOperationException("cannot send to more than one handler");

            handlers[0].Invoke(command);
        }

        public void RegisterHandler<T>(Action<T> handler) where T : Message
        {
            List<Action<Message>> handlers;
            if (!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<Message>>();
                _routes.Add(typeof(T), handlers);
            }
            handlers.Add(DelegateAdjuster.CastArgument<Message, T>(x => handler(x)));
        }
    }
}
