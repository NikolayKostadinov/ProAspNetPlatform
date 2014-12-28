using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models.IdentityModels;

namespace Users.ViewModels.IdentityViewModels
{
    public class RoleEditViewModel
    {
        public AppRole Role { get; set; }
        public IEnumerable<AppUser> Members { get; set; }
        public IEnumerable<AppUser> NonMembers { get; set; }
    }
}