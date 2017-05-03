using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoU2016.Models
{
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        //You can turn off IDENTITY Property by using the DatabaseGeneratedOption.None
        //You have the following 3 options:
        //  Computed: Database generates a value when a row is inserted or updated.
        //  Identity: Database generates a value when a row is inserted. 
        //  None: Database does not geerate a value.
        [Display(Name="Course Number")]
        public int CourseID { get; set; }

        [StringLength(50, MinimumLength =3)]
        [Required] 
        public string Title { get; set; }

        [Range(0,5)]
        public int Credits { get; set; }

        [Display(Name ="Department")]
        public int DepartmentID { get; set; }//FK: 1 department: many courses


        //=============== NAVIGATION PROPERTIES ===============
        //1 course: many enrollments
        public virtual ICollection<Enrollment> Enrollments { get; set; }

        //1 course: many instructors
        public virtual ICollection<CourseAssignment> Assignments { get; set; }

        public virtual Department Department { get; set; }

        //Calculated Propery
        //Return the CourseID and Course title
        public string CourseIdTitle
        {
            get
            {
                return CourseID + ": " + Title;
                //1: Chemistry
                //2: Math
            }
        }



    }
}