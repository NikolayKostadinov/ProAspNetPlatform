using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.DataModels;

namespace Models.IdentityModels
{

    public abstract class UserCustomeProps : IdentityUser
    {
        //additionalProperties will go here
        public bool IsGoogleAuthenticatorEnabled { get; set; } 
        public string GoogleAuthenticatorSecretKey { get; set; }


    }
}
