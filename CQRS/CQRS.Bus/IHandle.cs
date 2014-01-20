using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Bus
{
    public interface IHandle<T>
    {
        void Handle(T message);
    }
}
