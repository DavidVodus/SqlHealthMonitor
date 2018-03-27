using System.Collections.Generic;
using System.Linq;
using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.DAL.Models.WebPages;

namespace SqlHealthMonitor.BLL.Services
{
    public interface IPageService
    {
        /// <summary>
        /// base on page,create missing columnPreference in rootPreference.view. 
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="userId"></param>
        List<PageReviewViewModel> ReadPages(string userId);

        PageBase ReadPageProperties(string userId, string controllerName, string startActionName);
        //void DeletePage(GridPageViewModel pageModel);

        void SaveUserPreferences(PageBase page);
    }
}