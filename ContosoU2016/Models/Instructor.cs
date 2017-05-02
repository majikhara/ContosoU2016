using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models
{

    public class Instructor : Person
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        //================== NAVIGATION PROPERTIES ==================
        //public virtual ICollection<CourseAssignment> Courses { get; set; }
        //public virtual OfficeAssignment OfficeAssignment { get; set; }

    }
}
