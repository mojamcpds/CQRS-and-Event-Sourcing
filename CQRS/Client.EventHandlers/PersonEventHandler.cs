using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Events;
using CQRS.Bus;
using CQRS.Model;
using CQRS.Reporting;

namespace Client.EventHandlers
{
    public class PersonEventHandler : IHandle<PersonCreatedEvent>, IHandle<PersonUpdatedEvent>, IHandle<PersonDeletedEvent>
    {
        /// <summary>
        /// Event Handler Class should be responsible for Save/Update/Delete Reporting Side Data
        /// </summary>
        /// 
        private readonly IReportingRepository _reportingRepository;

       
        public PersonEventHandler(IReportingRepository reportingrepository)
        {
            _reportingRepository = reportingrepository;
        }

        /// <summary>
        /// Save Reporting Data
        /// </summary>
        /// <param name="reportingrepository"></param>
        public void Handle(PersonCreatedEvent evt)
        {
            var person = new Person
            {
                Id = evt.Id,
                LastName = evt.LastName,
                FirstName = evt.FirstName,
                Age = evt.Age
            };
            _reportingRepository.Add<Person>(person);
        }

        /// <summary>
        /// Update Reporting Data
        /// During Update first check wheather previous data and new data are same or different
        /// </summary>
        /// <param name="evt"></param>
        public void Handle(PersonUpdatedEvent evt)
        {
            var person = _reportingRepository.GetById<Person>(evt.Id);
            if (person.FirstName != evt.FirstName)
                person.FirstName = evt.FirstName;
            
            if (person.LastName != evt.LastName)
                person.LastName = evt.LastName;

            if (person.Age != evt.Age)
                person.Age = evt.Age;

            _reportingRepository.Modify<Person>(person);

        }

        /// <summary>
        /// Delete Reporting Data
        /// </summary>
        /// <param name="evt"></param>
        public void Handle(PersonDeletedEvent evt)
        {
            _reportingRepository.Delete<Person>(evt.Id);
        }
    }
}
