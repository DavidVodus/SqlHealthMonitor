using System.Collections.Generic;
using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;

namespace SqlHealthMonitor.BLL.Models.Identity.UserLogin
{
    public class ApplicationUserPageViewModel: PageViewModelBase
    {
        public IList<ApplicationUser> Users { get; set; }
    }
}
