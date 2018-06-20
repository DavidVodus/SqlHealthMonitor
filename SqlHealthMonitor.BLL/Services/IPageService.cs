using System.Collections.Generic;
using System.Linq;
using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.DAL.Models.WebPages;

namespace SqlHealthMonitor.BLL.Services
{
    public interface IPageService
    {
        /// <summary>
        /// Get All web pages representation from the database 
        /// </summary>
        /// <param name="userId"></param>
        List<PageSettingViewModel> ReadPages(string userId);

   

        void SaveUserPreferences(PageBase page);
    }
}