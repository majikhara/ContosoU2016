using ContosoU2016.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoU2016.Data
{
    //Create the SchoolContext (database context for our University database)
    public class SchoolContext: DbContext //Derives from the System.Data.Entity.DBContext class
    {
        //constructor
        public SchoolContext(DbContextOptions<SchoolContext> options):base(options)
        {


        }

        //Specifying Entity Sets - corresponding to database tables and each single
        //entity corresponds to a row in a table
        public DbSet<Person> People { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        /*
         * When the database is created, EF creates tables that have names the same as the DbSet
         * property names.  Property names for collections are typically plural (Students rather than
         * Student).  Developers disagree about whether table names should be pluralized or not.
         * For this demo, let's override the default behaviour.
         */ 
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
            modelBuilder.Entity<Instructor>().ToTable("Instructor");
        }

    }
}
