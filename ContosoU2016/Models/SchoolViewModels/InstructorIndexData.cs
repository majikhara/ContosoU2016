using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models.SchoolViewModels
{
    public class InstructorIndexData
    {
        /*
         *  The Instructors Index page will show data from three different tables, so for this
         *  reason we are creating the InstructorIndexData ViewModel.  It will include three properties
         *  each holding the dataa for one of the following tables:
         *  - Instructor
         *  - Course
         *  - Enrollment
         */

        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Enrollment> Enrollments { get; set; }

    }
}
