using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Events;
using CQRS.Core;

namespace Client.Domains
{
    public class PersonDomain : AggregateRoot
    {
        /// <summary>
        /// Handle all domain Logic Here
        /// By This class we will apply value to event
        /// Then event will apply to event handler
        /// </summary>
        
        private Guid _id;
        
        public override Guid Id
        {
            get { return _id; }
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }

        private void Apply(PersonCreatedEvent e)
        {
            _id = e.Id;
            LastName = e.LastName;
            FirstName = e.FirstName;
            Age = e.Age;
        }

        public PersonDomain() { }

        public PersonDomain(Guid id, string lastName, string firstName, int age)
        {
            ApplyChange(new PersonCreatedEvent(id, lastName, firstName, age));
        }

        public void Update(string lastName, string firstName, int age)
        {
            ApplyChange(new PersonUpdatedEvent(_id, lastName, firstName, age));
        }

        public void Delete()
        {
            ApplyChange(new PersonDeletedEvent(_id));
        }

    }
}
