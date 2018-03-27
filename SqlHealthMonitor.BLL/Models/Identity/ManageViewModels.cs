using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SqlHealthMonitor.BLL.Models.WebPages;

namespace SqlHealthMonitor.BLL.Models.Identity
{
    public class IndexViewModel : PageViewModelBase
    {
        public bool BrowserRemembered { get; set; }
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
    }

    public class ManageLoginsViewModel : PageViewModelBase
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel : PageViewModelBase
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel : PageViewModelBase
    {
        [DataType(DataType.Password)]
        [Display(Name = "Confirmpassword", ResourceType = typeof(Resources.Global))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordsNotMatch",
            ErrorMessageResourceType = typeof(Resources.Global))]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Newpassword", ResourceType = typeof(Resources.Global))]
        public string NewPassword { get; set; }
    }

    public class ChangePasswordViewModel : PageViewModelBase
    {
        public string Id { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmpassword", ResourceType = typeof(Resources.Global))]
        [Compare("NewPassword", ErrorMessageResourceName = "PasswordsNotMatch",
            ErrorMessageResourceType = typeof(Resources.Global))]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Newpassword", ResourceType = typeof(Resources.Global))]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Currentpassword", ResourceType = typeof(Resources.Global))]
     
        public string OldPassword { get; set; }
    }


    public class AddPhoneNumberViewModel : PageViewModelBase
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel : PageViewModelBase
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel : PageViewModelBase
    {
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string SelectedProvider { get; set; }
    }

}