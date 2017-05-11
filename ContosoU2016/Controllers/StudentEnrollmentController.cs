using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoU2016.Data;
using ContosoU2016.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
namespace ContosoU2016.Controllers
{
    [Authorize]//simply need to be logged in
    [Authorize(Roles = "student")]//need to be logged in and part of student role
    public class StudentEnrollmentController : Controller
    {
        private readonly SchoolContext _context;
        private readonly UserManager<ApplicationUser> _userManager; //dcowan: need Identity users
        public StudentEnrollmentController(SchoolContext context, UserManager<ApplicationUser> userManager)//dcowan: instantiate the userManager
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: StudentEnrollment
        public async Task<IActionResult> Index()
        {
            //dcowan: retrieve currently logged in student
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                //not logged in
                return NotFound(); //TO DO: return Error View
                //return View("Error");
            }
            //Locate logged in user (student) within the Student Entity
            var student = await _context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.Email == user.Email); //associate identity user --> student
                                                                   //using email property           
                                                                   //some may need to associate by id
                                                                   //you may also want to associate by id instead of Email:
                                                                   //1.================= Courses Enrolled ============================
            var studentEnrollments = _context.Enrollments
                .Include(c => c.Course)
                .Include(c => c.Student)
                .Where(c => c.StudentID == student.ID)
                .AsNoTracking();
            //Get the student name for display in view
            ViewData["StudentName"] = student.FullName;
            //2.================= Courses Available ===========================
            string query = "SELECT CourseID, Credits, Title, DepartmentID " +
                           "FROM Course WHERE CourseID NOT IN " +
                           "(SELECT DISTINCT CourseID FROM Enrollment WHERE StudentID = {0})";
            //Building a RAW SQL Query using LINQ (Language INtegrated Query)
            var courses = _context.Courses
                .FromSql(query, student.ID)
                .AsNoTracking();
            //ViewData["Courses"] = courses.ToList();
            ViewBag.Courses = courses.ToList();

            return View(await studentEnrollments.ToListAsync());
        }
        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        //GET: Enroll
        public async Task<IActionResult> Enroll(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            //get currently logged in student
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return NotFound();
                //TO DO: return error view
            }
            //Locate the logged in user (student) within the Student Entity
            var student = await _context.Students
                .Include(s => s.Enrollments).ThenInclude(s => s.Course)
                .AsNoTracking()
                .SingleOrDefaultAsync(s => s.Email == user.Email);
            //Return Student ID to view using ViewData
            ViewData["StudentID"] = student.ID; //for hidden field in form (so we know who they are)
            //retrieve this student's current enrollment
            //(for comparison with course they want to enroll)
            var studentEnrollments = new HashSet<int>(_context.Enrollments
                .Include(e => e.Course)
                .Include(e => e.Student)
                .Where(e => e.StudentID == student.ID)
                .Select(e => e.CourseID));//Only select courseID
            //conversion from int? to int is not possible (need int)
            int currentCourseID;
            if (id.HasValue)//id is the method parameter
            {
                currentCourseID = (int)id;
            }
            else
            {
                currentCourseID = 0;
            }
            //end conversion fix
            //Situation where student tries to enroll in same course
            if (studentEnrollments.Contains(currentCourseID))
            {
                //Student is trying to enroll in a course, already enrolled in.
                //Send a model state error back to view
                ModelState.AddModelError("AlreadyEnrolled", "You are already enrolled in this course!");
            }
            //Situation where student tries to enroll in non-existant course
            var course = await _context.Courses.SingleOrDefaultAsync(c => c.CourseID == id);
            //if course was not instantiated because no course id was found based on param coming in
            //for example StudentEnrollment/Enroll/9000
            //9000 is not a valid course
            //return not found
            if (course == null)
            {
                return NotFound();
            }
            //otherwise return the view (with course entity)
            //for example StudentEnrollment/Enroll/1045
            return View(course);
        }
        //POST: Enroll
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Enroll([Bind("CourseID, StudentID")] Enrollment enrollment)
        {
            _context.Add(enrollment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        private bool EnrollmentExists(int id)
        {
            return _context.Enrollments.Any(e => e.EnrollmentID == id);
        }
    }
}