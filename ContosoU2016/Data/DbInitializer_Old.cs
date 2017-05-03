using ContosoU2016.Data;
using System;
using System.Linq;

namespace ContosoU2016.Models
{
    //mwilliams: Part 2 -  Create the Data Model
    //Add code to initialize the database with test data

    // Code will cause a database to be created when needed and loads test data into the new database
    public static class DbInitializer_Old
    {
        public static void Initialize(SchoolContext context)
        {
            //context.Database.EnsureCreated();// EnsureCreated method will automatically create the database.
            //Later we will handle model changes by using Code First Migrations to change the database schema 
            //instead of dropping and re-creating the database
            //https://docs.microsoft.com/en-us/aspnet/core/data/ef-mvc/complex-data-model#seed-the-database-with-test-data

            //================================================== STUDENTS ============================================//
            // Look for any students.
            if (context.Students.Any())
            {
                return;   // DB has been seeded
            }

            var students = new Student[]
            {
            new Student{FirstName="Carson",LastName="Alexander", Email="calexander@contoso.com",EnrollmentDate=DateTime.Parse("2016-09-01")},
            new Student{FirstName="Meredith",LastName="Alonso",Email="malonso@contoso.com", EnrollmentDate=DateTime.Parse("2016-09-01")},
            new Student{FirstName="Arturo",LastName="Anand",Email="aanand@contoso.com", EnrollmentDate=DateTime.Parse("2016-09-01")},
            new Student{FirstName="Gytis",LastName="Barzdukas",Email="gbarzdukas@contoso.com", EnrollmentDate=DateTime.Parse("2016-09-01")},
            new Student{FirstName="Yan",LastName="Li",Email="liyan@contoso.com",EnrollmentDate=DateTime.Parse("2016-09-01")},
            new Student{FirstName="Peggy",LastName="Justice",Email = "pjustice@contoso.com", EnrollmentDate=DateTime.Parse("2016-09-01")},
            new Student{FirstName="Laura",LastName="Norman",Email="lnorman@contoso.com", EnrollmentDate=DateTime.Parse("2016-09-01")},
            new Student{FirstName="Nino",LastName="Olivetto",Email="nolivetto@contoso.com",EnrollmentDate=DateTime.Parse("2016-09-01")}
            };
            foreach (Student s in students)
            {
                context.Students.Add(s);
            }
            context.SaveChanges();

            //================================================ INSTRUCTORS ============================================//
            var instructors = new Instructor[]
            {
                new Instructor{FirstName="Emily", LastName="Williams", Email="emilyallain.testing@gmail.com", HireDate=DateTime.Parse("2016-09-01")},
                new Instructor{FirstName="Johnny", LastName="Rico", Email="rico.testing@gmail.com", HireDate=DateTime.Parse("2016-09-08")}
            };
            foreach (Instructor i in instructors)
            {
                context.Instructors.Add(i);
            }
            context.SaveChanges();


            //================================================== COURSES ==============================================//
            var courses = new Course[]
            {
            new Course{CourseID=1050,Title="Chemistry",Credits=3,},
            new Course{CourseID=4022,Title="Microeconomics",Credits=3,},
            new Course{CourseID=4041,Title="Macroeconomics",Credits=3,},
            new Course{CourseID=1045,Title="Calculus",Credits=4,},
            new Course{CourseID=3141,Title="Trigonometry",Credits=4,},
            new Course{CourseID=2021,Title="Composition",Credits=3,},
            new Course{CourseID=2042,Title="Literature",Credits=4,}
            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();


            //================================================== ENROLLMENT  ==============================================//
            var enrollments = new Enrollment[]
            {
            new Enrollment{StudentID=1,CourseID=1050,Grade=Grade.A},
            new Enrollment{StudentID=1,CourseID=4022,Grade=Grade.C},
            new Enrollment{StudentID=1,CourseID=4041,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=1045,Grade=Grade.B},
            new Enrollment{StudentID=2,CourseID=3141,Grade=Grade.F},
            new Enrollment{StudentID=2,CourseID=2021,Grade=Grade.F},
            new Enrollment{StudentID=3,CourseID=1050},
            new Enrollment{StudentID=4,CourseID=1050,},
            new Enrollment{StudentID=4,CourseID=4022,Grade=Grade.F},
            new Enrollment{StudentID=5,CourseID=4041,Grade=Grade.C},
            new Enrollment{StudentID=6,CourseID=1045},
            new Enrollment{StudentID=7,CourseID=3141,Grade=Grade.A},
            };
            foreach (Enrollment e in enrollments)
            {
                context.Enrollments.Add(e);
            }
            context.SaveChanges();
        }
    }
}