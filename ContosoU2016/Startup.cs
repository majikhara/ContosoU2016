using ContosoU2016.Data;
using ContosoU2016.Models;
using ContosoU2016.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ContosoU2016
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //eallain: School services
            services.AddDbContext<SchoolContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            /*
             * ASP.NET Services can be configured with the following lifetimes:
             * ===============
             * == Transient ==
             * ===============
             * Transient lifetime services are created each time they are requested.
             * This lifetime workds best for lightweight, stateless services
             * 
             * ================
             * ==== Scoped ====
             * ================
             * Scoped lifetime services are created once per request.
             *  
             * ===============
             * == Singleton ==
             * ===============
             * Singleton lifetime services are created the first time they are requested
             * (or when ConfigureServices is run if you specify the instance there) and
             * then every subsequent request will use the same instance
             * 
             */

            //eallain:  Service for Seeding admin user and roles
            services.AddTransient<AdministratorSeedData>();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, 
            SchoolContext context, //eallain add SchoolContext Middleware to Pipeline
            AdministratorSeedData seeder) //eallain add AdministratorSeed Middleware to Pipeline 
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            //Initialize the database with SEED data
            DbInitializer.Initialize(context);
            /* The first time you run the application the database will be created and seeded with
             * test data.  Whenever you change your data model, you can delete the database, update your
             * seed method and start fresh with a new database the same way.
             * 
             * Later we will modify the database when the data model changes, without deleting and
             * re-creating it using CODE FIRST MIGIRATIONS
             * 
             */

            //Seed the Administrator and roles
            await seeder.EnsureSeedData();
        }
    }
}
