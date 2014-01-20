using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace CQRS.NServiceBus
{
    [Serializable]
    public class NServiceBusEventMessage<TEvent> : INServiceBusEventMessage
        where TEvent : Event
    {
        public TEvent RealEvent { get; set; }

        public string Header { get; set; }

        Event INServiceBusEventMessage.RealEvent
        {
            get { return RealEvent; }
            set { RealEvent = (TEvent)value; }
        }
    }
}
