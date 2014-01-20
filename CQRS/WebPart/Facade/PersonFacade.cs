using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Client.Commands;
using CQRS.Bus;
using CQRS.Model;
using CQRS.Reporting;
using WebPart.ViewModel;

namespace WebPart.Facade
{
    public class PersonFacade
    {
        private ICommandBus _bus;
        private IReportingRepository _readmodel;

        public PersonFacade(ICommandBus bus, IReportingRepository repo)
        {
            this._bus = bus;
            this._readmodel = repo;
        }
        
        public void Save(VMPerson person)
        {
            _bus.Send(new CreatePersonCommand(Guid.NewGuid(), person.LastName, person.FirstName, person.Age));
        }

        public void Update(VMPerson person)
        {
            _bus.Send(new UpdatePersonCommand(person.Id, person.LastName, person.FirstName, person.Age));
        }

        public VMPerson GetById(Guid id)
        {
            var person = _readmodel.GetById<Person>(id);
            VMPerson personviewmodel = new VMPerson();
            if (person != null)
            {

                personviewmodel.Id = person.Id;
                personviewmodel.LastName = person.LastName;
                personviewmodel.FirstName = person.FirstName;
                personviewmodel.Age = person.Age;
                
            }
            return personviewmodel;
        }

        public void Delete(Guid id)
        {
            var person = this.GetById(id);
            _bus.Send(new DeletePersonCommand(person.Id));
        }

        public IEnumerable<VMPerson> GetAll()
        {
            var persons = _readmodel.GetItems<Person>();
            List<VMPerson> vmList = new List<VMPerson>();
            
            foreach (Person person in persons)
            {
                var vm = new VMPerson();
                vm.Id = person.Id;
                vm.LastName = person.LastName; ;
                vm.FirstName = person.FirstName;
                vm.Age = person.Age;
                
                vmList.Add(vm);
            }
            return vmList;
        }
    }
}