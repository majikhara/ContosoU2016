using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Models.SchoolViewModels
{
    public class AssignedCourseData
    {
        /*
         * To provide a list of course checkboxes with CourseID and Title as well as an indicator
         * that the instructor is assigned or not assigned to a particular course, we are creating this
         * ViewModel class  
         * 
         */
        public int CourseID { get; set; } //For the CourseID
        public string Title { get; set; } //For the Course Title
        public bool Assigned { get; set; } //For (is instructor Assigned or NOT assigned to this course?)


    }
}
