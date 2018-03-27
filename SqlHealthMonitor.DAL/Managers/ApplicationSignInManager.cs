using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;

namespace SqlHealthMonitor.DAL.Managers
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public class ApplicationRoleStore
        : RoleStore<ApplicationRole, string, ApplicationUserRole>,
        IQueryableRoleStore<ApplicationRole, string>,
        IRoleStore<ApplicationRole, string>, IDisposable
        {
            public ApplicationRoleStore()
                : base(new IdentityDbContext())
            {
                DisposeContext = true;
            }

            public ApplicationRoleStore(DbContext context)
                : base(context)
            {
            }
        }

        public class ApplicationUserStore
               : UserStore<ApplicationUser, ApplicationRole, string,
                   ApplicationUserLogin, ApplicationUserRole,
                   ApplicationUserClaim>, IUserStore<ApplicationUser, string>,
               IDisposable
        {
            public ApplicationUserStore()
                : this(new IdentityDbContext())
            {
                DisposeContext = true;
            }

            public ApplicationUserStore(DbContext context)
                : base(context)
            {
            }
        }
    }
}
