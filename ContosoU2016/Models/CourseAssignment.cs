using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models
{
    public class CourseAssignment
    {
        //[Key]
        public int InstructorID { get; set; } //Composite PK, FK to Instructor Entity
        //[Key]
        public int CourseID { get; set; } //Composite PK, FK to Course Entity

        /*
         * We could label both properties with [Key] attribute to create a composite PK
         * but we will do it using Fluent-API within the SchoolContext Class
         */

        //=================== NAVIGATION PROPERTIES ======================

        //Many-to-Many (This is the junction or join table) between Instructor and Course 
        //Many Instructors teaching many courses
        //1 Course many Course Assignments
        //1 Instructor many Course Assignments
        public virtual Instructor Instructor { get; set; }
        public virtual Course Course { get; set; }
    }
}
