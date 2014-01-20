using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CQRS.Configuration;
using Ninject;
namespace WebPart
{
    public class PersonBootStrapper
    {
        public void BootStrapTheApplication()
        {
            BusBootStrapper.BootStrap(WebApiApplication.PersonKarnel);

        }

        public static void BootStrap()
        {
            new PersonBootStrapper().BootStrapTheApplication();
        }
    }
}