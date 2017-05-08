using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContosoU2016.Data;
using ContosoU2016.Models;
using ContosoU2016.Helpers;
using ContosoU2016.Models.SchoolViewModels;

namespace ContosoU2016.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolContext _context;

        public StudentController(SchoolContext context)
        {
            _context = context;    
        }
        
        // GET: Student
        public async Task<IActionResult> Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            //sortOrder:  for sorting
            //searchString:  for searching
            //currentFilter: to keep current search
            //page:  for paging (optional argument)

            ViewData["CurrentSort"] = sortOrder;

            // return View(await _context.Students.ToListAsync());
            // eallain: add paging, sorting and filtering functionality.
            var students = from s in _context.Students
                           select s; //SELECT FirstName, LastName, Email, EnrollmentDate FROM Students 
            // Part 1:  Sorting

            //Default Sort LastName
            ViewData["LNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "lname_desc" : ""; 

            //Other Sort Orders | Toggle between ascending and descending
            ViewData["FNameSortParm"] = sortOrder == "fname" ? "fname_desc" : "fname";
            ViewData["EmailSortParm"] = sortOrder == "email" ? "email_desc" : "email";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";

            //rewrite one of the coalescing if
            //if(sortOrder == "fname")
            //{
            //    sortOrder = "fname_desc";
            //}
            //else
            //{
            //    sortOrder = "fname";
            //}
            // ViewData["FNameSortParm"] = sortOrder

            //Part 2:  Filtering
            if (searchString == null)
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1; //Start on first page.
                /*
                 * If the searchString is changed during paging, the page has to be reset to 1
                 * because the new filter can result in a different data to display.
                 */ 
            }

            ViewData["CurrentFilter"] = searchString;
            

            if (!string.IsNullOrEmpty(searchString))
            {
                //User entered a search criteria -> search by last name or first name
                students = students.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString));
            }


            //Apply the Sorting
            switch(sortOrder)
            {
                case "lname_desc":
                    students = students.OrderByDescending(s => s.LastName);
                    break;
                    //FirstName
                case "fname":
                    students = students.OrderBy(s => s.FirstName);
                    break;
                case "fname_desc":
                    students = students.OrderByDescending(s => s.FirstName);
                    break;
                    //Email
                case "email":
                    students = students.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    students = students.OrderByDescending(s => s.Email);
                    break;
                    //Date
                case "date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;

                default: //LastName ascending
                    students = students.OrderBy(s => s.LastName);
                    break;              

            }
            //eallain: Changed to use paginated list
            //return View(await students.ToListAsync());
            int pageSize = 5;

            return View(await PaginatedList<Student>.CreateAsync(students, page ?? 1, pageSize));
            //double question mark (??) is the null-coalescing operator
            //page the value of page unless page is null, in which it is assigned to the value of 1
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
                /*
                 * Status Codes:
                 * Success:
                 * return Ok() <- HTTP Status code 200
                 * return Created() <- HTTP status code 201
                 * return NoContent() <- HTTP status code 204
                 * 
                 * Client Error:
                 * return BadRequest() <- HTTP status code 400
                 * return Unauthorized() <- HTTP status 401
                 * return NotFound() <- HTTP status 404
                 */ 
            }

            //eallain:  Update to include related data (enrollment)
            //var student = await _context.Students
            //    .SingleOrDefaultAsync(m => m.ID == id);

            var student = await _context.Students
            .Include(s=>s.Enrollments)
            .ThenInclude(c=>c.Course)
            .AsNoTracking()
            .SingleOrDefaultAsync(m => m.ID == id);
            /*
            * ====================================================== No-tracking queries =======================================================
            * When a database context retrieves table rows and creates entity objects that represent them, by default it keeps track of whether 
            * the entities in memory are in sync with what's in the database. The data in memory acts as a cache and is used when you update an entity. 
            * This caching is often unnecessary in a web application because context instances are typically short-lived 
            * (a new one is created and disposed for each request) and the context that reads an entity is typically disposed before that entity is used again.
            * 
            * Ref:  https://docs.microsoft.com/en-us/ef/core/querying/tracking
            */
            
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentDate,LastName,FirstName,Email")] Student student)
        {
            //eallain:  remove the id from the bind attributes: ID is the PK as well as IDENTITY
            try
            {
                    if (ModelState.IsValid)
                    {
                        _context.Add(student);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
            }
            catch (DbUpdateException)
            {

                ModelState.AddModelError("", "Unable to save changes. " + "Please try again.");
            }
        
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                
                return NotFound();
            }

            //Find the student to be updated
            var studentToUpdate = await _context.Students.SingleOrDefaultAsync(s => s.ID == id);

            //Try to update this student
            if(await TryUpdateModelAsync<Student>
                (studentToUpdate, "", s=>s.FirstName, s=>s.LastName, s=> s.Email, s=>s.EnrollmentDate))
            {
                try
                {
                    await _context.SaveChangesAsync(); //Save changes back to database
                    return RedirectToAction("Index");  //redirect user back to index route
                }                   
                catch (DbUpdateException /*ex*/)
                {
                    //Log the error (by uncommenting the "ex" variable) and write to a log file.
                    //return a ModelStateError
                    ModelState.AddModelError("", "Unable to save changes." + "Try again");
                }
            }
            //return the view and attach the studentToUpdate model
            return View(studentToUpdate);
          
        }

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError=false)
        {
            //eallain
            /* This code accepts an optional boolean parameter that indicates whether the method was called after
             * a delete failure (failure saving changes back to database).
             * When it is called by the HTTPPost Delete method in response to database error, this parameter
             * will be passed in set to true.
             * 
             */

            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .SingleOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            //eallain:  return update error if necessary
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed! Try again later.";
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.AsNoTracking().SingleOrDefaultAsync(m => m.ID == id);

            //eallain:  check if student exists
            if(student == null)
            {
                return RedirectToAction("Index");
            }
            try
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
	        catch (DbUpdateException)
            {
                //return user to the Delete GET method passing it the current student (ID)
                //and a flag argument set to TRUE for representing and error saving
                return RedirectToAction("Delete", new {id=id, saveChangesError=true});
            
	        }
    }
        //eallain
        //GET Student/Stats
        public async Task<IActionResult> Stats()
        {
            //Populate the EnrollmentDateGroup ViewModel with Student Statistics
            IQueryable<EnrollmentDateGroup> data =
                from student in _context.Students //FROM Students
                group student by student.EnrollmentDate into dateGroup //GROUP by EnrollmentDate
                select new EnrollmentDateGroup //SELECT EnrollmentDate COUNT(*) as StudentCount
                {
                    EnrollmentDate = dateGroup.Key,
                    StudentCount = dateGroup.Count()
                };

            return View(await data.AsNoTracking().ToListAsync()); //AsNoTracking is not necessary
        }


        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
