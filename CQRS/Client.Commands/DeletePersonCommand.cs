using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace Client.Commands
{
    [Serializable]
    public class DeletePersonCommand:Command
    {
        public Guid Id { get; private set; }

        public DeletePersonCommand(Guid id)
        {
            Id = id;
        }
    }
}
