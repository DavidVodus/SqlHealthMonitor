
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SqlHealthMonitor.BLL.Models.Identity;
using SqlHealthMonitor.BLL.Models.Identity.UserLogin;
using SqlHealthMonitor.DAL.Managers;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;

namespace SqlHealthMonitor.WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesAdminController : SqlHealthMonitor.Infrastructure.ControllerBase
    {
        //
        // GET: /Roles/Create
        public ActionResult Create()
        {
            return View("Create", "_Layout",new RoleViewModel());
        }

        //
        // POST: /Roles/Create
        [HttpPost]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole(roleViewModel.Name);
                var roleresult = await RoleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                    return View("Create", "_Layout",new RoleViewModel());
                }
                return RedirectToAction("Index");
            }
            return View("Create", "_Layout",new RoleViewModel());
        }

        //
        // GET: /Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            List<ApplicationRole> roles = new List<ApplicationRole>();
            roles.Add(role);
            var modelfiles = new ApplicationRolePageViewModel { Roles = RoleManager.Roles.ToList() };
            return View("Delete", "_Layout", modelfiles);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id, string deleteUser)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var role = await RoleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
                IdentityResult result;
                if (deleteUser != null)
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                else
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View("DeleteConfirmed", "_Layout",new ApplicationRolePageViewModel());
                }
                return RedirectToAction("Index");
            }
            return View("DeleteConfirmed", "_Layout", new ApplicationRolePageViewModel());
        }

        //
        // GET: /Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            List<ApplicationRole> roles = new List<ApplicationRole>(); 
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in UserManager.Users.ToList())
            {
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }
            roles.Add(role);
            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            var modelfiles = new ApplicationRolePageViewModel { Roles = RoleManager.Roles.ToList() };
            return View("Details", "_Layout", modelfiles);
        }

        //
        // GET: /Roles/Edit/Admin
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
            return View("Edit", "_Layout", roleModel);
         
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Id")] RoleViewModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(roleModel.Id);
                role.Name = roleModel.Name;
                await RoleManager.UpdateAsync(role);
                return RedirectToAction("Index");
            }
            return View("Edit", "_Layout",new RoleViewModel());
        }

        //
        // GET: /Roles/
        public ActionResult Index()
        {
            var modelfiles = new ApplicationRolePageViewModel { Roles = RoleManager.Roles.ToList() };
            return View("Index", "_Layout", modelfiles );
        }
    }
}
