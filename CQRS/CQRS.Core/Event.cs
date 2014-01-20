using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Core
{
    [Serializable]
    public class Event : Message
    {
        public int Version;
    }
}
