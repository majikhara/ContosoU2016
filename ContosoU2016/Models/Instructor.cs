using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models
{
    public class Instructor : Person
    {
        public DateTime HireDate { get; set; }
        
        //================== NAVIGATION PROPERTIES ==================
        //public virtual ICollection<CourseAssignment> Courses { get; set; }
        //public virtual OfficeAssignment OfficeAssignment { get; set; }

    }
}
