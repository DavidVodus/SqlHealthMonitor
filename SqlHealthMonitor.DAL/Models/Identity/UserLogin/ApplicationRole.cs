using System;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SqlHealthMonitor.DAL.Models.Identity.UserLogin
{
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        public ApplicationRole(string name)
            : this()
        {
            Name = name;
        }

        // Add any custom Role properties/code here
    }
}
