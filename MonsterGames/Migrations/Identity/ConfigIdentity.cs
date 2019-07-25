namespace MonsterGames.Migrations.Identity
{
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;
  using MonsterGames.Models;
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;

    internal sealed class ConfigIdentity : DbMigrationsConfiguration<MonsterGames.Models.ApplicationDbContext>
    {
        public ConfigIdentity()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\Identity";
            ContextKey = "MonsterGames.Models.ApplicationDbContext";
        }

        protected override void Seed(MonsterGames.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
          if (!context.Roles.Any(r => r.Name.Equals(ApplicationConstants.Admin)))
          {
            using (var store = new RoleStore<IdentityRole>(context))
            {
              using (var manager = new RoleManager<IdentityRole>(store))
              {
                var role = new IdentityRole { Name = ApplicationConstants.Admin };
                manager.Create(role);
              }
            }
          }
          // create admin role
          if (!context.Users.Any(r => r.UserName.Equals("poki@me.com")))
          {
            using (var store = new UserStore<ApplicationUser>(context))
            {
              using (var manager = new UserManager<ApplicationUser>(store))
              {
                var user = new ApplicationUser
                {
                  UserName = "poki@me.com",
                  Email = "poki@me.com"
                };
                manager.Create(user, "#123Abc");
                manager.AddToRole(user.Id, ApplicationConstants.Admin);
              }
            }
          }
          // make a guest, not in any role
          if (!context.Users.Any(r => r.UserName.Equals("guest@email.com")))
          {
            using (var store = new UserStore<ApplicationUser>(context))
            {
              using (var manager = new UserManager<ApplicationUser>(store))
              {
                var user = new ApplicationUser
                {
                  UserName = "guest@email.com",
                  Email = "guest@email.com"
                };
                manager.Create(user, "#123Abcd");
              }
            }
          }
        }
    }
}
