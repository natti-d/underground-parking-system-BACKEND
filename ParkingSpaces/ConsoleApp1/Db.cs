using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Db
    {
        public static List<User> GetUsers()
        {
            return new List<User>() { 
                new User() { 
                    Name = "pesho",
                    Age = 19
                },
                new User() {
                    Name = "gencho",
                    Age = 20
                },
                new User() {
                    Name = "stoqn",
                    Age = 60
                }
            };
        }
    }
}
