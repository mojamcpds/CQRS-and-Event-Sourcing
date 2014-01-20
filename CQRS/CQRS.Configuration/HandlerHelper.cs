using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Bus;
using CQRS.Core;
using CQRS.Utils;

namespace CQRS.Configuration
{
    public class HandlerHelper
    {
        public static IDictionary<Type, IList<Type>> GetHandlers()
        {
            IDictionary<Type, IList<Type>> handlers = new Dictionary<Type, IList<Type>>();

            AssemblyScanner.ScanAssembliesFor(typeof(IHandle<>))
                .ToList()
                .ForEach(h => AddItems(handlers, h));

            return handlers;
        }

        public static IEnumerable<Type> GetCommands()
        {
            return AssemblyScanner.ScanAssembliesFor<Command>().ToList();
        }

        public static IEnumerable<Type> GetEvents()
        {
            return AssemblyScanner.ScanAssembliesFor<Event>().ToList();
        }

        private static void AddItems(IDictionary<Type, IList<Type>> dictionary, Type type)
        {
            var handlerInterfaces = type.GetInterfaces()
                .Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IHandle<>));


            foreach (Type handlerInterface in handlerInterfaces)
            {
                var message = handlerInterface.GetGenericArguments().First();

                if (!dictionary.ContainsKey(message))
                    dictionary.Add(message, new List<Type>());

                dictionary[message].Add(type);
            }

        }
    }
}
