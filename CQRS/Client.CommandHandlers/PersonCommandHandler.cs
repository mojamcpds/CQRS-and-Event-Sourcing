using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Client.Commands;
using Client.Domains;
using CQRS.Bus;
using CQRS.EventStore;

namespace Client.CommandHandlers
{
    public class PersonCommandHandler:IHandle<CreatePersonCommand>,IHandle<UpdatePersonCommand>,IHandle<DeletePersonCommand>
    {
        /// <summary>
        /// CommandHandler is use to Save/update/delete domain
        /// </summary>
        private readonly IDomainRepository<PersonDomain> _domainRepository;

        public PersonCommandHandler(IDomainRepository<PersonDomain> domainRepository)
        {
            this._domainRepository = domainRepository;
        }

        public void Handle(CreatePersonCommand command)
        {
            var person = new PersonDomain(command.Id,command.LastName,command.FirstName,command.Age);
            var aggregateId = -1;
            _domainRepository.Save(person, aggregateId);
            
        }

        public void Handle(UpdatePersonCommand command) {
            var person = _domainRepository.GetById(command.Id);
            person.Update(command.LastName, command.FirstName, command.Age);
            _domainRepository.Save(person, -1);

        }

        public void Handle(DeletePersonCommand command)
        {
            var person = _domainRepository.GetById(command.Id);
            person.Delete();
            _domainRepository.Save(person, -1);
        }

    }
}
