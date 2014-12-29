using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Models.IdentityModels
{
    public abstract class RoleCustomeProps : IdentityRole
    {
         public RoleCustomeProps()
            :base()
        {
        }

         public RoleCustomeProps(string name)
             : base(name) 
        {
        }

        public bool IsAvailableForAdministrators { get; set; }
    }
}
