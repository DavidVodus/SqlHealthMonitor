using System;
using SqlHealthMonitor.BLL.Models;
using SqlHealthMonitor.DAL.Helpers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;
using System.Text;

using SqlHealthMonitor.DAL.Managers;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;
using SqlHealthMonitor.DAL.Models.WebPages;
using SqlHealthMonitor.DAL.Repositories;

namespace SqlHealthMonitor.BLL.Helpers
{
    public static class Database
    {
        public static void CreateAdmin()
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            const string name = "Admin";
            const string password = "123456";
            const string roleName = "Admin";

            //Create Role Admin if it does not exist
            var role = roleManager.FindByName(roleName);
            if (role == null)
            {
                role = new ApplicationRole(roleName);
                var roleresult = roleManager.Create(role);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                try
                {
                    user = new ApplicationUser {UserName = name, Email = ""};
                    var result = userManager.Create(user, password);
                    result = userManager.SetLockoutEnabled(user.Id, false);
                }
                catch (DbEntityValidationException e)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        sb.AppendLine(string.Format(
                            "Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State));
                        foreach (var ve in eve.ValidationErrors)
                        {
                            sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage));
                        }
                    }
                    throw new DbEntityValidationException(sb.ToString(), e);
                }
            }

            // Add user admin to Role Admin if not already added
            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains(role.Name))
            {
                var result = userManager.AddToRole(user.Id, role.Name);
            }

        }

        public static void CreateViews(DbContext context)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var admin = userManager.FindByName("Admin");
            if(admin==null)
                throw new InvalidOperationException("Admin doesnt exist in Database");
            PageRepository pageRepository = new PageRepository(context);
            List<PageBase> pages = new List<PageBase>();

            pages.Add(new HomePage()
            {
                PageName = "Home",
                StartActionName = "Index",
                ControllerName = "Home",
                ApplicationUser = admin,
                ApplicationUserId=admin.Id

            });
            pages.Add(new SqlDashBoardPage()
            {
                PageName = "SqlDashBoard",
                StartActionName = "Index",
                ControllerName = "SqlDashBoard",
                ApplicationUser = admin,
                ApplicationUserId = admin.Id
            });
          
            foreach (var createdPage in pages)
            {
                //find page type in database
                var databasePages = pageRepository.GetQueryable()
                    .Where(x => x.ControllerName == createdPage.ControllerName).ToList();
                //iterate those pages
                foreach (var databasePage in databasePages)
                {
                    //if that is main page,change properties ,except controllername due to identification of this page
                    //that must not be changed!
                    if (databasePage.ApplicationUser == null)
                    {
                        databasePage.PageName = createdPage.PageName;
                        databasePage.StartActionName = createdPage.StartActionName;
                    }
                 
                    pageRepository.Update(databasePage);

                }
                //if it is new page in the system, add it to database
                if(!databasePages.Any())
                {
                    pageRepository.Add(createdPage,x=> new { x.ApplicationUser });
                }
                pageRepository.Save();
            }

        
        }


     

    }
}

