using ContosoU2016.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoU2016.Data
{
    public class AdministratorSeedData
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministratorSeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task EnsureSeedData()
        {
            //Check if we have existing admin user (admin@contoso.com)  - do not want to rerun this part
            if(await _userManager.FindByEmailAsync("admin@contoso.com")==null)
            {
                //Create 'admin' role 
                var adminRole = await _roleManager.FindByNameAsync("admin");  //does role exist?
                if (adminRole == null)
                {
                    //admin role does not exist - create it
                    adminRole = new IdentityRole("admin");
                    await _roleManager.CreateAsync(adminRole);
                }

                //Create the 'admin' user
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@contoso.com",
                    Email = "admin@contoso.com"
                };

                await _userManager.CreateAsync(adminUser, "AdminPass@123456");
                await _userManager.SetLockoutEnabledAsync(adminUser, false); //admin cannot be locked out

                //assign adminUser to admin rol
                IdentityResult result = await _userManager.AddToRoleAsync(adminUser, "admin");
            }

            //Create 'student' role
            var studentRole = await _roleManager.FindByNameAsync("student");
            if (studentRole == null)
            {
                //student role does not exist - create it
                studentRole = new IdentityRole("student");
                await _roleManager.CreateAsync(studentRole);
            }

            //Create 'instructor' role
            var instructorRole = await _roleManager.FindByNameAsync("instructor");
            if (instructorRole == null)
            {
                //student role does not exist - create it
                instructorRole = new IdentityRole("instructor");
                await _roleManager.CreateAsync(instructorRole);
            }
        }
    }
}
