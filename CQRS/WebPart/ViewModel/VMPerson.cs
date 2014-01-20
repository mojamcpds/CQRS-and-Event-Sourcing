using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPart.ViewModel
{
    public class VMPerson
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
    }
}