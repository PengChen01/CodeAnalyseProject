using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Product2.Test
{
    public class User
    {
        public string Name
        {
            get;
            set;
        }
        public string Age
        {
            get;
            set;
        }
        public User(string name,string age)
        {
            Name = name;
            Age = age;
        }
        public User()
        {
        }
    }
}
