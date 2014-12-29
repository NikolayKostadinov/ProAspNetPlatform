namespace Users.Infrastructure.IdentityInfrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Data;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Models.IdentityModels;

    public class AppRoleManager : RoleManager<AppRole>, IDisposable
    {
        public AppRoleManager(RoleStore<AppRole> store)
            :base(store)
        {
            
        }

        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> oprions,
            IOwinContext context)
        {
            return new AppRoleManager(new RoleStore<AppRole>(context.Get<DataContext>()));
        }
    }
}