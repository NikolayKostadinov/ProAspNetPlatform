namespace Data
{
    using Data.Migrations;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.IdentityModels;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DataContext:IdentityDbContext<AppUser>
    {
        public DataContext() 
            : base("IdentityDb") 
        { 
        }

        static DataContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext,Configuration>());
        }

        public static DataContext Create()
        {
            return new DataContext();
        }
    }
 
}
