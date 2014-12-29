using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.DataModels;

namespace Models.IdentityModels
{
    public abstract class UserCustomeProps:IdentityUser
    {
        //additionalProperties will go here

        public Cities City { get; set; }
    }
}
