using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SqlHealthMonitor.BLL.Models.WebPages;

namespace SqlHealthMonitor.BLL.Models.Identity
{
    public class RoleViewModel : PageViewModelBase
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName", ResourceType = typeof(Resources.Global))]
        public string Name { get; set; }
    }

    public class EditUserViewModel : PageViewModelBase
    {
        //[DisplayFormat(ConvertEmptyStringToNull = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        public string Id { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Global))]
        public string UserName { get; set; }
    }
}