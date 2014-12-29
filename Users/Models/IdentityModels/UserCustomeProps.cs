using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Models.DataModels;

namespace Models.IdentityModels
{
<<<<<<< HEAD
    public abstract class UserCustomeProps : IdentityUser
    {
        //additionalProperties will go here
        public string GoogleAuthenticatorSecretKey { get; set; }
=======
    public abstract class UserCustomeProps:IdentityUser
    {
        //additionalProperties will go here

        public Cities City { get; set; }
>>>>>>> f3b07ea5dc9aaa7841c016eb3f0dcb9e18295e02
    }
}
