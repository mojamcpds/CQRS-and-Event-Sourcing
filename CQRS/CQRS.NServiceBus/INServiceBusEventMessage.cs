using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;
using NServiceBus;

namespace CQRS.NServiceBus
{
    public interface INServiceBusEventMessage : IMessage
    {
        Event RealEvent { get; set; }
        string Header { get; set; }
    }
}
