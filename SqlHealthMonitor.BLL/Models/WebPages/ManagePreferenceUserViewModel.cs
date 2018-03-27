using System.Web;

namespace SqlHealthMonitor.BLL.Models.WebPages
{
    public  class ManagePreferenceUserViewModel: PageViewModelBase
    {
        public string SelectedControllerName { get; set; }
        public string SelectedStartActionName { get; set; }

    }
}
