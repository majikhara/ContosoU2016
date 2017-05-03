using ContosoU2016.Data;
using System;
using System.Linq;

namespace ContosoU2016.Models
{
    //mwilliams: Part 2 -  Create the Data Model
    //Add code to initialize the database with test data

    // Code will cause a database to be created when needed and loads test data into the new database
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {

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

            //================================================== INSTRUCTORS  ============================================//
            var instructors = new Instructor[]
            {
                new Instructor {FirstName="Marc",LastName="Williams",HireDate=DateTime.Parse("1996-01-31"), Email="mwilliams@faculty.contoso.com" },
                new Instructor {FirstName="Frank",LastName="Bekkering",HireDate=DateTime.Parse("1997-08-30") , Email="fbekkering@contoso.com"},
                new Instructor {FirstName = "Kim",LastName = "Abercrombie",HireDate = DateTime.Parse("1995-03-11"), Email="kabercrombie@faculty.contoso.com"},
                new Instructor {FirstName = "Fadi",LastName = "Fakhouri", HireDate = DateTime.Parse("2002-07-06"), Email="ffakhouri@faculty.contoso.com"},
                new Instructor {FirstName = "Roger", LastName = "Harui", HireDate = DateTime.Parse("1998-07-01"), Email="rharui@faculty.contoso.com"},
                new Instructor {FirstName = "Candace",LastName = "Kapoor", HireDate = DateTime.Parse("2001-01-15"), Email="ckapoor@faculty.contoso.com"},
                new Instructor {FirstName = "Roger",LastName = "Zheng",HireDate = DateTime.Parse("2004-02-12"), Email="rzheng@faculty.contoso.com"}
            };

            foreach (Instructor i in instructors)
            {
                context.Instructors.Add(i);
            }
            context.SaveChanges();

            //================================================== DEPARTMENTS  ============================================//
            var departments = new Department[]
           {
                new Department { Name = "English", Budget = 350000, CreatedDate = DateTime.Parse("2017-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Abercrombie").ID },
                new Department { Name = "Mathematics", Budget = 100000,CreatedDate = DateTime.Parse("2017-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Fakhouri").ID },
                new Department { Name = "Engineering", Budget = 350000,CreatedDate = DateTime.Parse("2017-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Harui").ID },
                new Department { Name = "Economics",   Budget = 100000,CreatedDate = DateTime.Parse("2017-09-01"),
                    InstructorID  = instructors.Single( i => i.LastName == "Kapoor").ID }
           };

            foreach (Department d in departments)
            {
                context.Departments.Add(d);
            }
            context.SaveChanges();
            //================================================== COURSES ==============================================//
            var courses = new Course[]
            {
                new Course {CourseID = 1050, Title = "Chemistry",      Credits = 3,
                    DepartmentID = departments.Single( s => s.Name == "Engineering").DepartmentID
                },
                new Course {CourseID = 4022, Title = "Microeconomics", Credits = 3,
                    DepartmentID = departments.Single( s => s.Name == "Economics").DepartmentID
                },
                new Course {CourseID = 4041, Title = "Macroeconomics", Credits = 3,
                    DepartmentID = departments.Single( s => s.Name == "Economics").DepartmentID
                },
                new Course {CourseID = 1045, Title = "Calculus",       Credits = 4,
                    DepartmentID = departments.Single( s => s.Name == "Mathematics").DepartmentID
                },
                new Course {CourseID = 3141, Title = "Trigonometry",   Credits = 4,
                    DepartmentID = departments.Single( s => s.Name == "Mathematics").DepartmentID
                },
                new Course {CourseID = 2021, Title = "Composition",    Credits = 3,
                    DepartmentID = departments.Single( s => s.Name == "English").DepartmentID
                },
                new Course {CourseID = 2042, Title = "Literature",     Credits = 4,
                    DepartmentID = departments.Single( s => s.Name == "English").DepartmentID
                },
            };
            foreach (Course c in courses)
            {
                context.Courses.Add(c);
            }
            context.SaveChanges();

            //================================================== OFFICE ASSIGNMENTS ==============================================//
            var officeAssignments = new OfficeAssignment[]
            {
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Fakhouri").ID,
                    Location = "Smith 17" },
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Harui").ID,
                    Location = "Gowan 27" },
                new OfficeAssignment {
                    InstructorID = instructors.Single( i => i.LastName == "Kapoor").ID,
                    Location = "Thompson 304" },
            };

            foreach (OfficeAssignment o in officeAssignments)
            {
                context.OfficeAssignments.Add(o);
            }
            context.SaveChanges();


            //================================================== COURSE ASSIGNMENTS  ==============================================//
            var courseInstructors = new CourseAssignment[]
               {
                    new CourseAssignment {
                        CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
                        InstructorID = instructors.Single(i => i.LastName == "Kapoor").ID
                        },
                    new CourseAssignment {
                        CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
                        InstructorID = instructors.Single(i => i.LastName == "Harui").ID
                        },
                    new CourseAssignment {
                        CourseID = courses.Single(c => c.Title == "Microeconomics" ).CourseID,
                        InstructorID = instructors.Single(i => i.LastName == "Zheng").ID
                        },
                    new CourseAssignment {
                        CourseID = courses.Single(c => c.Title == "Macroeconomics" ).CourseID,
                        InstructorID = instructors.Single(i => i.LastName == "Zheng").ID
                        },
                    new CourseAssignment {
                        CourseID = courses.Single(c => c.Title == "Calculus" ).CourseID,
                        InstructorID = instructors.Single(i => i.LastName == "Fakhouri").ID
                        },
                    new CourseAssignment {
                        CourseID = courses.Single(c => c.Title == "Trigonometry" ).CourseID,
                        InstructorID = instructors.Single(i => i.LastName == "Harui").ID
                        },
                    new CourseAssignment {
                        CourseID = courses.Single(c => c.Title == "Composition" ).CourseID,
                        InstructorID = instructors.Single(i => i.LastName == "Abercrombie").ID
                        },
                    new CourseAssignment {
                        CourseID = courses.Single(c => c.Title == "Literature" ).CourseID,
                        InstructorID = instructors.Single(i => i.LastName == "Abercrombie").ID
                        },
               };

            foreach (CourseAssignment ci in courseInstructors)
            {
                context.CourseAssignments.Add(ci);
            }
            context.SaveChanges();



            //================================================== ENROLLMENT  ==============================================//
            var enrollments = new Enrollment[]
                        {
                new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID,
                    Grade = Grade.A
                },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Microeconomics" ).CourseID,
                    Grade = Grade.C
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alexander").ID,
                    CourseID = courses.Single(c => c.Title == "Macroeconomics" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.LastName == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Calculus" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                        StudentID = students.Single(s => s.LastName == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Trigonometry" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Alonso").ID,
                    CourseID = courses.Single(c => c.Title == "Composition" ).CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry" ).CourseID
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Anand").ID,
                    CourseID = courses.Single(c => c.Title == "Microeconomics").CourseID,
                    Grade = Grade.B
                    },
                new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Barzdukas").ID,
                    CourseID = courses.Single(c => c.Title == "Chemistry").CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Li").ID,
                    CourseID = courses.Single(c => c.Title == "Composition").CourseID,
                    Grade = Grade.B
                    },
                    new Enrollment {
                    StudentID = students.Single(s => s.LastName == "Justice").ID,
                    CourseID = courses.Single(c => c.Title == "Literature").CourseID,
                    Grade = Grade.B
                    }
                        };

            foreach (Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments.Where(
                    s =>
                            s.Student.ID == e.StudentID &&
                            s.Course.CourseID == e.CourseID).SingleOrDefault();
                if (enrollmentInDataBase == null)
                {
                    context.Enrollments.Add(e);
                }
            }
            context.SaveChanges();
        }
    }
}