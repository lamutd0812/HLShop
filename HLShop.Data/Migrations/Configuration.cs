 using HLShop.Model.Models;
 using Microsoft.AspNet.Identity;
 using Microsoft.AspNet.Identity.EntityFramework;

 namespace HLShop.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HLShop.Data.HLShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HLShop.Data.HLShopDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new HLShopDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new HLShopDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "huulam",
                Email = "lamutd0812@gmail.com",
                EmailConfirmed = true,
                BirthDay = DateTime.Now,
                Fullname = "Nguyen Huu Lam"
            };

            manager.Create(user, "1234564$");
            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin"});
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByEmail("lamutd0812@gmail.com");

            manager.AddToRoles(adminUser.Id, new string[] {"Admin", "User"});

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

        }
    }
}
