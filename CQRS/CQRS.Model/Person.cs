using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CQRS.Model
{
    public class Person
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int Age { get; set; }
        
    }
}
