using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace Client.Commands
{
    [Serializable]
    public class UpdatePersonCommand:Command
    {
        public Guid Id { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public int Age { get; private set; }

        public UpdatePersonCommand(Guid id, string lastname, string firstname, int age)
        {
            Id = id;
            LastName = lastname;
            FirstName = firstname;
            Age = age;
        }
    }
}
