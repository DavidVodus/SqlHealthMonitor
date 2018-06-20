using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SqlHealthMonitor.DAL.Models.WebPages;
using SqlHealthMonitor.DAL.Models.Widgets;

namespace SqlHealthMonitor.DAL.Models.Identity.UserLogin
{

    public sealed class ApplicationUser
           : IdentityUser<string, ApplicationUserLogin,
           ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUser()
        {
        
            Id = Guid.NewGuid().ToString();
          
        // Add any custom User properties/code here
    }

        // [Association("ClientClientCodes","Id", "RootPreferenceId")]
        [ForeignKey("ApplicationUserId")]
        public ICollection<WidgetBase> Widgets { get; set; }
        [ForeignKey("ApplicationUserId")]
        public  ICollection<SqlServerData> SqlServers { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ICollection<PageBase> PagesPreferences { get; set; }
        public string Language { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser,string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
          
            return userIdentity;
        }
    }
}
