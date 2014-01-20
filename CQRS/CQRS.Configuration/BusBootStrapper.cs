using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CQRS.Bus;
using Ninject;

namespace CQRS.Configuration
{
    public class BusBootStrapper
    {
        private readonly IRouteMessages _router;
        private readonly IKernel _kernel;

        private MethodInfo _createActionMethod;
        private static MethodInfo _registerMethod;

        private BusBootStrapper(IKernel kernel)
        {
            _router = kernel.Get<IRouteMessages>();
            _kernel = kernel;
        }

        public void BootStrapTheBus()
        {
            //var router = MvcApplication.InventoryKernal.Get<IRouteMessages>();

            _createActionMethod = GetType().GetMethod("CreateAction");
            _registerMethod = _router.GetType().GetMethod("RegisterHandler");

            var commands = HandlerHelper.GetCommands();
            var handlers = HandlerHelper.GetHandlers();
            var events = HandlerHelper.GetEvents();

            foreach (var command in commands)
            {
                IList<Type> commandHandlerTypes;
                if (!handlers.TryGetValue(command, out commandHandlerTypes))
                    throw new Exception(string.Format("No command handlers found for command '{0}'", command.FullName));

                foreach (var commandHandler in commandHandlerTypes)
                {
                    var injectedHandler = GetCorrectlyInjectedHandler(commandHandler);
                    var action = CreateTheProperAction(command, injectedHandler);
                    RegisterTheCreatedActionWithTheMessageRouter(_router, command, action);
                }
            }

            foreach (var @event in events)
            {
                IList<Type> commandHandlerTypes;
                if (!handlers.TryGetValue(@event, out commandHandlerTypes))
                    throw new Exception(string.Format("No event handlers found for event '{0}'", @event.FullName));

                foreach (var commandHandler in commandHandlerTypes)
                {
                    var injectedHandler = GetCorrectlyInjectedHandler(commandHandler);
                    var action = CreateTheProperAction(@event, injectedHandler);
                    RegisterTheCreatedActionWithTheMessageRouter(_router, @event, action);
                }
            }
        }

        private object GetCorrectlyInjectedHandler(Type handler)
        {
            return _kernel.Get(handler);
        }

        private void RegisterTheCreatedActionWithTheMessageRouter(IRouteMessages router, Type messageType, object action)
        {
            _registerMethod.MakeGenericMethod(messageType).Invoke(router, new[] { action });
        }

        private object CreateTheProperAction(Type commandType, object commandHandler)
        {
            return _createActionMethod.MakeGenericMethod(commandType, commandHandler.GetType()).Invoke(this, new[] { commandHandler });
        }

        public Action<TMessage> CreateAction<TMessage, THandler>(THandler handler)
            where TMessage : class
            where THandler : IHandle<TMessage>
        {
            return message => _kernel.Get<THandler>().Handle(message);
        }

        public static void BootStrap(IKernel kernel)
        {
            new BusBootStrapper(kernel).BootStrapTheBus();
        }


    }
}
