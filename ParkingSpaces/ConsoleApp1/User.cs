using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Person
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; } // MiddleName can be null
        public string LastName { get; set; }
    }
}
