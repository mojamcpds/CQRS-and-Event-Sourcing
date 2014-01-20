using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using NServiceBus;

namespace CQRS.NServiceBus
{
    public static class ConfigureCqrsNServiceBus
    {
        public static ConfigEventBus AgileWorkshopCqrsNServiceBus(this Configure configure, IKernel kernel)
        {
            return new ConfigEventBus(configure, kernel);
        }
    }
}
