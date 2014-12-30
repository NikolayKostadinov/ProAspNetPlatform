using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Users.Infrastructure.Helpers
{
    public enum SignInStatus
    {
        Success,
        LockedOut,
        RequiresTwoFactorAuthentication,
        Failure
    }
}