using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SqlHealthMonitor.BLL.Models.Identity;
using SqlHealthMonitor.BLL.Models.Identity.UserLogin;
using AutoMapper;
using SqlHealthMonitor.DAL.Managers;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;
using Microsoft.AspNet.Identity;

namespace SqlHealthMonitor.WEB.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : SqlHealthMonitor.Infrastructure.ControllerBase
    {
      
        public UsersAdminController()
        {
        }
        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
            return View("Create", "_Layout",new RegisterViewModel());
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = userViewModel.UserName, Email = userViewModel.Email};
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                            return View("Create", "_Layout", userViewModel);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
                    return View("Create", "_Layout", userViewModel);

                }
                return RedirectToAction("Index", new { Message = ManageMessageId.UserCreated });
            }
            ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
            return View("Create", "_Layout", userViewModel);
        }
        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View("ChangePassword", "_Layout", new ChangePasswordViewModel());
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("ChangePassword", "_Layout", model);
            }
            var resultRemove = await UserManager.RemovePasswordAsync(model.Id);
            var result = await UserManager.AddPasswordAsync(model.Id, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(model.Id);
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View("ChangePassword", "_Layout", model);
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add(user);
            var modelfiles = new ApplicationUserPageViewModel { Users = users };
            return View("Delete", "_Layout",modelfiles);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(" DeleteConfirmed", "_Layout",new ApplicationUserPageViewModel());
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.UserDeleted });
            }
            return View(" DeleteConfirmed", "_Layout", new ApplicationUserPageViewModel());
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);
            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add(user);
            var modelfiles = new ApplicationUserPageViewModel { Users = users };
            return View("Details", "_Layout", modelfiles);
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            return View("Edit", "_Layout", new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "UserName,Email,Id")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.UserName = editUser.UserName;
                user.Email = editUser.Email;

                var userRoles = await UserManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };
                var result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View("Edit", "_Layout", editUser);
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View("Edit", "_Layout", editUser);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.UsersInformationUpdated });
            }
            ModelState.AddModelError("", "Something failed.");
           return await Edit(editUser.Id);
           // return View("Edit", "_Layout");
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
               message == ManageMessageId.ChangePasswordSuccess ? "Password has been changed."
               : message == ManageMessageId.UserCreated ? "User Created"
               : message == ManageMessageId.UserDeleted ? "User Deleted."
                  : message == ManageMessageId.UsersInformationUpdated ? "Information about user updated."
               : "";

            var modelfiles = new ApplicationUserPageViewModel { Users = await UserManager.Users.ToListAsync() };
            return View("Index", "_Layout", modelfiles);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error,
            UserCreated,
            UsersInformationUpdated,
            UserDeleted
        }
    }
}
