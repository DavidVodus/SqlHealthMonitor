using System.Collections.Generic;
using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.DAL.Models.Identity.UserLogin;

namespace SqlHealthMonitor.BLL.Models.Identity.UserLogin
{
    public class ApplicationRolePageViewModel: PageViewModelBase
    {
        public IList<ApplicationRole> Roles { get; set; }
    }
}
