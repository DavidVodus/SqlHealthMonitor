using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SqlHealthMonitor.BLL.Models.WebPages;


namespace SqlHealthMonitor.BLL.Models.Identity
{

    public class SendCodeViewModel : PageViewModelBase
    {
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public string SelectedProvider { get; set; }
    }

    public class VerifyCodeViewModel : PageViewModelBase
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        public string Provider { get; set; }

        [Display(Name = "Rememberthisbrowser", ResourceType = typeof(Resources.Global))]
        public bool RememberBrowser { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class ForgotViewModel : PageViewModelBase
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel : PageViewModelBase
    {
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
       [Display(Name = "Password", ResourceType = typeof(Resources.Global))]
        public string Password { get; set; }

        [Display(Name = "Rememberthisbrowser", ResourceType = typeof(Resources.Global))]
        public bool RememberMe { get; set; }

        [Required]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Global))]
        public string UserName { get; set; }
    }

    public class RegisterViewModel : PageViewModelBase
    {
        [DataType(DataType.Password)]
        [Display(Name = "Confirmpassword", ResourceType = typeof(Resources.Global))]
        [Compare("Password", ErrorMessageResourceName = "PasswordsNotMatch",
            ErrorMessageResourceType = typeof(Resources.Global))]
        public string ConfirmPassword { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Resources.Global))]
        public string Password { get; set; }

        [Required]
        [Display(Name = "UserName", ResourceType = typeof(Resources.Global))]
        public string UserName { get; set; }
    }

    public class ResetPasswordViewModel : PageViewModelBase
    {
        public string Code { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmpassword", ResourceType = typeof(Resources.Global))]
        [Compare("Password", ErrorMessageResourceName = "PasswordsNotMatch",
            ErrorMessageResourceType = typeof(Resources.Global))]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(Resources.Global))]
        public string Password { get; set; }
    }

    public class ForgotPasswordViewModel : PageViewModelBase
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}