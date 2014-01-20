using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CQRS.Core;

namespace CQRS.Bus
{
    public interface ICommandBus
    {
        void Send<T>(T command) where T : Command;

    }
}
