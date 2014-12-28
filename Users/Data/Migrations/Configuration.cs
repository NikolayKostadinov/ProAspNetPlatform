namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.IdentityModels;

    public sealed class Configuration : DbMigrationsConfiguration<Data.DataContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Data.DataContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        /// <summary>
        /// Performs the initial setup.
        /// </summary>
        /// <param name="context">The context.</param>
        private void PerformInitialSetup(DataContext context)
        {
            var userMgr = new UserManager<AppUser>(new UserStore<AppUser>(context));
            var roleMgr = new RoleManager<AppRole>(new RoleStore<AppRole>(context));

            string roleName = "Administrators";
            string userName = "Administrator";
            string password = "MySecret";
            String email = "admin@example.com";
            
            if (!roleMgr.RoleExists(roleName))
            {
                roleMgr.Create(new AppRole(roleName));
            }

            AppUser user = userMgr.FindByName(userName);
            if (user == null)
            {
                userMgr.Create(new AppUser()
                {
                    UserName = userName,
                    Email = email,
                }, password);
                user = userMgr.FindByName(userName);
            }

            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }
        }
    }
}