using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace Client.Events
{
    [Serializable]
    public class PersonCreatedEvent:Event
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }

        public PersonCreatedEvent(Guid id, string lastname, string firstname, int age)
        {
            Id = id;
            LastName = lastname;
            FirstName = firstname;
            Age = age;
        }
    }
}
