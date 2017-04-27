using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models
{
    public abstract class Person
    {
        //eallain: Create the data models
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }

        //FullName: Calculated Property with a get accessor only
        //          *will not get generated in in database
        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }


    }
}
