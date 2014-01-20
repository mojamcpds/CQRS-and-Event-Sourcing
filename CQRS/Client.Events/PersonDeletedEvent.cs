using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace Client.Events
{
    [Serializable]
    public class PersonDeletedEvent:Event
    {
        public Guid Id { get; set; }

        public PersonDeletedEvent(Guid id)
        {
            Id = id;
        }
    }
}
