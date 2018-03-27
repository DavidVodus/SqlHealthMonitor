using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlHealthMonitor.BLL.Models.WebPages;
using SqlHealthMonitor.BLL.Models.WebPages.Components;
using SqlHealthMonitor.DAL.Models.WebPages;

using SqlHealthMonitor.DAL.Repositories;

namespace SqlHealthMonitor.BLL.Services
{
    using System;
    using System.Linq;
    using AutoMapper;
    using System.Collections.Generic;


        public class PageService : ServiceBase, IPageService
        {

            private MapperConfiguration _config;


            private IPageRepository _pageRep;

            public PageService(IPageRepository pageRep)
            {

                _pageRep = pageRep;
                _config = new MapperConfiguration(cfg =>
                {

                    cfg.CreateMap<PageBase, PageReviewViewModel>().ForMember(y => y.PageType, opt =>
                        opt.MapFrom(o => o.GetType().Name));

                    cfg.CreateMissingTypeMaps = true;
                });
            }

            protected override Type LogPrefix => GetType();

            //public IQueryable<PageBase> ReadPages() => _pageRep.GetQueryable();


            /// <summary>
            /// base on page,create missing columnPreference in rootPreference.view. 
            /// </summary>
            /// <param name="controllerName"></param>
            /// <param name="userId"></param>
            public List<PageReviewViewModel> ReadPages(string userId)
            {
                if (userId == null)
                    return null;
            var mapper = new Mapper(_config);
                var page = _pageRep.GetQueryable()
                    .Where(o =>  o.ApplicationUserId == userId);
           
             var pageViewModel = mapper.DefaultContext.Mapper.Map<List<PageReviewViewModel>>(page);
                return pageViewModel;
            }
            public PageBase ReadPageProperties(string userId, string controllerName, string startActionName)
            {
                var mapper = new Mapper(_config);
                var page = _pageRep
                    .GetQueryable().SingleOrDefault(o => o.ApplicationUserId == userId&& 
                    o.ControllerName==controllerName&& o.StartActionName==startActionName);
                //List<dynamic> pageViewModel = mapper.DefaultContext.Mapper.Map<List<dynamic>>(page);
                return page;
            }

            //public void DeletePage(GridPageViewModel pageModel)
            //{
            //    var mapper = new Mapper(_config);
            //    var page = mapper.DefaultContext.Mapper.Map<GridPage>
            //        (pageModel);
            //    _pageRep.Attach(page);
            //    _pageRep.Delete(page);
            //    _pageRep.Save();
            //}



            public void SaveUserPreferences(PageBase page)
            {

                //var mapper = new Mapper(_config);
                //var columnPreferences = mapper.DefaultContext.Mapper.Map<IList<GridColumnDefinition>>
                //    (page);
                _pageRep.Update(page);
                _pageRep.Save();

            }
        }
    }


