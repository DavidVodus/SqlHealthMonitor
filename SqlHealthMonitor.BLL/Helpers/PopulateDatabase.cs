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
using SqlHealthMonitor.BLL.Models.NetworkData;
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
                //var user = new ApplicationUser { UserName = userViewModel.UserName, Email = userViewModel.Email, PagePreferences = new PagePreferences() };
                //var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);
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
            //DbContext test= ContextManager.GetDbContext("NetworkFlowRepositoryContext");
            // var users = test.Set<ApplicationUser>();
            // var user = users.First();
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var admin = userManager.FindByName("Admin");
            if(admin==null)
                throw new InvalidOperationException("Admin doesnt exist in Database");
            PageRepository pageRepository = new PageRepository(context);
            //GridPageRepository gridPageRepository = new GridPageRepository(context);
            //GridColumnDefinitionRepository columnRepository = new GridColumnDefinitionRepository(context);
       
            List<PageBase> pages = new List<PageBase>();
            //var oc = (test as IObjectContextAdapter).ObjectContext;
            //pages.Add(new GridPage()
            //{

            //    Columns = EntityModelBuilder.CreateColumnModel(typeof(SwitchInterfaceViewModel)),
            //    PageName = "CableBook",
            //    StartActionName = "Index",
            //    ControllerName = "CableBook_",
            //    ApplicationUser = admin
            //});

            //pages.Add(new GridPage()
            //{

            //    Columns = EntityModelBuilder.CreateColumnModel(typeof(LogViewModel)),
            //    PageName = "Logs",
            //    StartActionName = "Index",
            //    ControllerName = "Log",
            //    ApplicationUser = admin

            //});

            pages.Add(new HomePage()
            {
                PageName = "Home",
                StartActionName = "Index",
                ControllerName = "Home",
                ApplicationUser = admin

            });
            pages.Add(new SqlDashBoardPage()
            {
                PageName = "SqlDashBoard",
                StartActionName = "Index",
                ControllerName = "SqlDashBoard",
                ApplicationUser = admin
            });
            //pages.Add(new GridPage()
            //{
            //    Columns = EntityModelBuilder.CreateColumnModel(typeof(SwitchInterfaceMacViewModel)),
            //    PageName = "CableBook_MacRecord",
            //    StartActionName = "Index",
            //    ControllerName = "CableBook_MacRecord",
            //    ApplicationUser = admin

            //});
            //pages.Add(new ToolPage()
            //{
            //    PageName = "CsvBaseReport",
            //    StartActionName = "Index",
            //    ControllerName = "CsvBaseReport",
            //    ApplicationUser = admin

            //});
            //pages.Add(new GridPage()
            //{
            //    Columns = EntityModelBuilder.CreateColumnModel(typeof(SwitchInterfaceMacProbeViewModel)),
            //    PageName = "CableBook_MacRecordProbe",
            //    StartActionName = "Index",
            //    ControllerName = "CableBook_MacRecordProbe",
            //    ApplicationUser = admin

            //});

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
                    //createdPage.PageId = page.PageId;
                    //if that is main gridpage, change columns definitions,if its users page,  just add new columns
                    //if (databasePage is GridPage)
                    //{
                    //    UpdateGridPageBase(createdPage as GridPage, databasePage as GridPage, columnRepository, context);
                    //    //if that is main page definition
                    //    //if (createdPage.ApplicationUser == null)
                    //    //    gridPageRepository.Update(createdPage as GridPage);
                    //    //TODO> update only system property on users page
                    //}
                    pageRepository.Update(databasePage);

                }
                //if it is new page in the system, add it to database
                if(!databasePages.Any())
                {
                    pageRepository.Add(createdPage);
                }
                pageRepository.Save();
            }

            ////If already exist, doesnt anything
            //foreach (var view in gridPageRepository.GetAll())
            //{
            //    if (view.Columns != null)
            //    {
            //        foreach (var column in view.Columns)
            //        {
            //            // var col = columnPreferenceRepository.GetQueryable().First(x => x.ViewPreferenceId == null && x.Column.GridColumnId == column.GridColumnId);
            //            if (columnPreferenceRepository.GetQueryable().Count(x=> x.Column.GridColumnId == column.GridColumnId) <= 0)
            //                columnPreferenceRepository.Add(new ColumnPreference { Column = column, PagePreferenceBaseId= view .PageId});
            //        }
            //        columnPreferenceRepository.Save();
            //    }
            //}
        }


     

    }
}

