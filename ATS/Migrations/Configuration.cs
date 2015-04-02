namespace ATS.Migrations
{
    using ATS.Infrastructure;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ATS.Infrastructure.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ATS.Infrastructure.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            //make a new user
            var user = new ApplicationUser()
            {
                UserName = "God",
                Email = "burton.wu@wforce.org",
                EmailConfirmed = true,
                FirstName = "Burton",
                LastName = "Wu",
                Level = 1,
                JoinDate = DateTime.Now.AddYears(-3)
            };

            manager.Create(user, "GodWOS1!");
            //initalize roles from scratch
            if (roleManager.Roles.Count() == 0)
            {
                roleManager.Create(new IdentityRole { Name = "SuperAdmin" });
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "SeniorManager" });
                roleManager.Create(new IdentityRole { Name = "RSM" });
                roleManager.Create(new IdentityRole { Name = "CSM" });
                roleManager.Create(new IdentityRole { Name = "Consultant" });
            }

            var adminUser = manager.FindByName("SuperPowerUser");
            //assign roles to new user
            manager.AddToRoles(adminUser.Id, new string[] { "SuperAdmin", "Admin" });
        }
    }
}
